/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Utils;
using Utils.UI;

namespace ZWave.Layers
{
    public class UdpJLinkDiscoveryService : UdpDevicesDiscoveryService<JlinkDeviceInfo>
    {
        public const string INITIAL_DISCOVERY_TEXT = "Discover";
        public const string TARGETED_DISCOVERY_TEXT = "DiscEx";
        public const string INITIAL_VERIFY_TEXT = "Found";
        public const string TARGETED_VERIFY_TEXT = "J-Link";
        public const int BROADCAST_PORT = 19020;
        public const int ADMIN_PORT = 4901;

        public override JlinkDeviceInfo[] DiscoverTcpDevices()
        {
            REQUEST_PORT_NO = BROADCAST_PORT;
            RESPONSE_PORT_NO = BROADCAST_PORT;
            UDP_RECEIVE_TIMEOUT = 100;

            var hostName = Dns.GetHostName();
            var ipEntry = Dns.GetHostEntry(hostName);
            var localIps = ipEntry.AddressList.Where(address => address.AddressFamily == AddressFamily.InterNetwork).ToArray();

            var RequestData = Encoding.ASCII.GetBytes(INITIAL_DISCOVERY_TEXT);
            byte[] reqBytes = new byte[64]; // should be exactly 64 bytes
            Array.Copy(RequestData, reqBytes, RequestData.Length);

            IPEndPoint ipBroad = new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT);
            var foundEndPoints = new List<JlinkDeviceInfo>();

            foreach (var address in localIps)
            {
                PerformUdpDiscovery(address, reqBytes, null, (bufferAnswer, endPoint) =>
                {
                    var answerText = Encoding.ASCII.GetString(bufferAnswer);
                    if (answerText.Contains(INITIAL_VERIFY_TEXT) &&
                        !foundEndPoints.Any(x => x.IPAddress.Equals(endPoint.Address)))
                    {
                        var serialNo = Tools.GetInt32(bufferAnswer.Skip(48).Take(4).Reverse().ToArray()).ToString();
                        var nickname = string.Empty;
                        if (bufferAnswer.Length > 32)
                        {
                            nickname = Encoding.ASCII.GetString(bufferAnswer.Skip(bufferAnswer.Length - 32).Where(x => x != 0).ToArray());
                        }
                        foundEndPoints.Add(new JlinkDeviceInfo(serialNo, null, endPoint.Address, nickname, true));
                        return true;
                    }
                    return false;
                });
            }
            //MakeTargetedDiscover(foundEndPoints);
            return foundEndPoints.Where(x => x.IsValid).ToArray();
        }

        public event EventHandler<JlinkDeviceInfo> OnFound;
        public async override Task DiscoverTcpDevicesAsync(IPAddress address)
        {
            REQUEST_PORT_NO = BROADCAST_PORT;
            RESPONSE_PORT_NO = BROADCAST_PORT;
            var RequestData = Encoding.ASCII.GetBytes(INITIAL_DISCOVERY_TEXT);
            byte[] reqBytes = new byte[64]; // should be exactly 64 bytes
            Array.Copy(RequestData, reqBytes, RequestData.Length);
            IPEndPoint ipBroad = new IPEndPoint(IPAddress.Broadcast, BROADCAST_PORT);
            await Task.Run(() =>
            {
                PerformUdpDiscoveryTask(address, reqBytes, (bufferAnswer, endPoint) =>
                {
                    try
                    {
                        var answerText = Encoding.ASCII.GetString(bufferAnswer);
                        if (answerText.Contains(INITIAL_VERIFY_TEXT))
                        {
                            var serialNo = Tools.GetInt32(bufferAnswer.Skip(48).Take(4).Reverse().ToArray()).ToString();
                            var nickname = string.Empty;
                            if (bufferAnswer.Length > 32)
                            {
                                nickname = Encoding.ASCII.GetString(bufferAnswer.Skip(bufferAnswer.Length - 32).Where(x => x != 0).ToArray());
                            }
                            OnFound?.Invoke(this, new JlinkDeviceInfo(serialNo, null, endPoint.Address, nickname, true));
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                });
            });
        }

        private void MakeTargetedDiscover(List<JlinkDeviceInfo> targets)
        {
            var requestData = Encoding.ASCII.GetBytes(TARGETED_DISCOVERY_TEXT);
            byte[] reqBytes = new byte[64]; // should be exactly 64 bytes
            Array.Copy(requestData, reqBytes, requestData.Length);

            foreach (var target in targets)
            {
                PerformUdpDiscovery(null, reqBytes, target.IPAddress, (bufferAnswer, endPoint) =>
                {
                    var answerText = Encoding.ASCII.GetString(bufferAnswer);
                    if (answerText.Contains(TARGETED_VERIFY_TEXT))
                    {
                        target.IsValid = true;
                        return true;
                    }
                    return false;
                });
            }
        }
    }

    public enum TestRoles
    {
        Unknown,
        UndefinedLink,
        SingleDevice,
        BoundDevice,
        Manipulator
    }

    public class JlinkDeviceInfo : EntityBase, IUdpDiscoverInfo
    {
        public IPAddress IPAddress { get; set; }
        public string SerialNo { get; set; }
        public bool IsValid { get; set; }

        private JlinkDeviceInfo _linkItem;
        private TestRoles _testRole;
        private string _nickname;
        private string _testBedName;
        private int _index;
        private string _chipSeries;
        private string _serialPort;

        public JlinkDeviceInfo(string serialNumber, string serialPort, IPAddress ipAddress, string nickname, bool isValid)
        {
            SerialNo = serialNumber;
            SerialPort = serialPort;
            IPAddress = ipAddress;
            Nickname = nickname;
            IsValid = isValid;
        }

        public string SName
        {
            get
            {
                return IPAddress?.ToString() ?? SerialPort;
            }
        }

        public IDataSource DataSource
        {
            get
            {
                IDataSource ret = null;
                if (IPAddress != null)
                {
                    ret = new SocketDataSource(IPAddress.ToString(), 4901);
                }
                else if (SerialPort != null)
                {
                    ret = new SerialPortDataSource(SerialPort);
                }
                return ret;
            }
        }

        public string SerialPort
        {
            get => _serialPort;
            set
            {
                _serialPort = value;
                Notify("SerialPort");
            }
        }

        public string ChipSeries
        {
            get => _chipSeries;
            set
            {
                _chipSeries = value;
                Notify("ChipSeries");
            }
        }

        public string Nickname
        {
            get => _nickname;
            set
            {
                _nickname = value;
                ParseNickname(_nickname, this);
                Notify("Nickname");
            }
        }

        public string TestBedName
        {
            get => _testBedName;
            set
            {
                _testBedName = value;
                Notify("TestBedName");
            }
        }

        public TestRoles TestRole
        {
            get => _testRole;
            set
            {
                _testRole = value;
                Notify("TestRole");
            }
        }

        public int Index
        {
            get => _index;
            set
            {
                _index = value;
                Notify("Index");
            }
        }

        public JlinkDeviceInfo LinkItem
        {
            get => _linkItem;
            set
            {
                _linkItem = value;
                Notify("LinkItem");
            }
        }
        public static string TestBedVersion => "Z1";
        public static string GetNickname(string testBedName, JlinkDeviceInfo wstk)
        {
            string role = null;
            switch (wstk.TestRole)
            {
                case TestRoles.Unknown:
                    role = "U";
                    break;
                case TestRoles.UndefinedLink:
                    role = "X";
                    break;
                case TestRoles.SingleDevice:
                    role = "A";
                    break;
                case TestRoles.Manipulator:
                    role = "M";
                    break;
                case TestRoles.BoundDevice:
                    role = "B";
                    break;
                default:
                    role = "U";
                    break;
            }
            var nickname = $"{TestBedVersion}{role}{wstk.Index:00}.{testBedName}";
            if (nickname.Length > 31)
            {
                nickname = nickname.Substring(0, 31);
            }
            return nickname;
        }

        public static void ParseNickname(string nickname, JlinkDeviceInfo wstkItem)
        {
            if (!string.IsNullOrWhiteSpace(nickname) && nickname.IndexOf('.') > 4 && nickname.StartsWith("Z1"))
            {
                int dotIndex = nickname.IndexOf('.');
                if (nickname[2] == 'U')
                {
                    wstkItem.TestRole = TestRoles.Unknown;
                }
                else if (nickname[2] == 'X')
                {
                    wstkItem.TestRole = TestRoles.UndefinedLink;
                }
                else if (nickname[2] == 'A')
                {
                    wstkItem.TestRole = TestRoles.SingleDevice;
                }
                else if (nickname[2] == 'B')
                {
                    wstkItem.TestRole = TestRoles.BoundDevice;
                }
                else if (nickname[2] == 'M')
                {
                    wstkItem.TestRole = TestRoles.Manipulator;
                }
                else
                {
                    wstkItem.TestRole = TestRoles.Unknown;
                }
                if (int.TryParse(nickname.Substring(3, dotIndex - 3), out int index))
                {
                    wstkItem.Index = index;
                }
                wstkItem.TestBedName = nickname.Substring(dotIndex + 1, nickname.Length - dotIndex - 1);
            }
            else
            {
                wstkItem.Index = 0;
                wstkItem.TestRole = TestRoles.Unknown;
                wstkItem.TestBedName = null;
                $"item {wstkItem.SerialNo} reset nickname: {nickname}"._DLOG();
            }
        }
    }
}
