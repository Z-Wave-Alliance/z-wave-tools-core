/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Linq;
using System.Net;
using Utils;
using ZWave.CommandClasses;
using ZWave.ZipApplication.Devices;

namespace ZWave.ZipApplication
{
    public class UnsolicitedDestinationConfigurator
    {
        private string LocalAddress => _localIpAddressBytes != null && _localIpAddressBytes.Length > 0 ? new IPAddress(_localIpAddressBytes).ToString() : null;
        private byte[] _localIpAddressBytes;
        private ushort _localPortNo;
        private ZipController _zipController;

        public Tuple<string, ushort> ConfiguredEndpoint { get; private set; }

        public UnsolicitedDestinationConfigurator(ZipController zipController)
        {
            _zipController = zipController;
        }

        public bool IsConfigured()
        {
            if (_zipController == null)
                return false;

            var localAddress = ((Dtls1ClientTransportClient)_zipController.TransportClient).DtlsClient.LocalEndpoint.Item1;
            if (!IPAddress.TryParse(localAddress, out IPAddress localIpAddress))
                return false;

            if (localIpAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                localIpAddress = localIpAddress.MapToIPv6();

            var localIpAddressBytes = localIpAddress.GetAddressBytes();
            var unsolGetRes = _zipController.RequestData(null, new COMMAND_CLASS_ZIP_GATEWAY.UNSOLICITED_DESTINATION_GET(),
               new COMMAND_CLASS_ZIP_GATEWAY.UNSOLICITED_DESTINATION_REPORT(), 2000);
            if (unsolGetRes && unsolGetRes.ReceivedData != null && unsolGetRes.ReceivedData.Length > 2)
            {
                var unsolReport = (COMMAND_CLASS_ZIP_GATEWAY.UNSOLICITED_DESTINATION_REPORT)unsolGetRes.ReceivedData;
                byte[] unsolPortData = unsolReport.unsolicitedDestinationPort;
                byte[] unsolAddressData = unsolReport.unsolicitedIpv6Destination;

                if (unsolPortData != null && unsolPortData.Length == 2 &&
                    unsolAddressData != null && unsolAddressData.Length == 16)
                {
                    var unsolPortNo = (ushort)Tools.GetInt32(unsolPortData);
                    if (unsolAddressData.SequenceEqual(localIpAddressBytes))
                        _localIpAddressBytes = localIpAddressBytes;

                    if (unsolPortNo != 0 && !string.IsNullOrEmpty(LocalAddress))
                    {
                        _localPortNo = unsolPortNo;
                        ConfiguredEndpoint = new Tuple<string, ushort>(LocalAddress, _localPortNo);
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AutoConfig()
        {
            ConfiguredEndpoint = null;
            return SetConfiguration(string.IsNullOrEmpty(LocalAddress) ? ((Dtls1ClientTransportClient)_zipController.TransportClient).DtlsClient.LocalEndpoint.Item1 : LocalAddress,
                _localPortNo == 0 ? (ushort)Constants.DtlsPortNo : _localPortNo);
        }


        public bool SetConfiguration(string address, ushort portNo)
        {
            if (string.IsNullOrEmpty(address) || portNo == 0)
                return false;

            if (!IPAddress.TryParse(address, out IPAddress localIpAddress))
                return false;

            if (_zipController.UnsolicitedDestinationSet(localIpAddress, portNo))
            {
                _localIpAddressBytes = localIpAddress.GetAddressBytes();
                _localPortNo = portNo;
                ConfiguredEndpoint = new Tuple<string, ushort>(LocalAddress, _localPortNo);
                return true;
            }
            
            return false;
        }
    }
}
