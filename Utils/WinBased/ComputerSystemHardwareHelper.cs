/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace Utils
{
    public class ComputerSystemHardwareHelper
    {
        #region Static Methods

        private static ManagementEventWatcher _eventWatcher;

        public static void InitEventWatcher()
        {
            if (_eventWatcher != null)
                CloseAndDisposeEventWatcher();

            //this watcher will check situations when devices added or removed
            _eventWatcher = new ManagementEventWatcher(@"SELECT * FROM Win32_DeviceChangeEvent");
            try
            {
                _eventWatcher.Start();
                _eventWatcher.EventArrived += eventWatcher_EventArrived;
            }
            catch (Exception ex)
            {
                "InitEventWatcher Error: {0}"._DLOG(ex.Message);
                CloseAndDisposeEventWatcher();
            }
        }

        private static void eventWatcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            foreach (PropertyData pData in e.NewEvent.Properties)
            {
                if (pData.Value != null)
                {
                }
                else
                {
                }
            }
        }

        public static void CloseAndDisposeEventWatcher()
        {
            if (_eventWatcher == null)
                return;

            try
            {
                _eventWatcher.Stop();
                _eventWatcher.Dispose();
                _eventWatcher = null;
            }
            catch (Exception ex)
            {
                "CloseAndDisposeEventWatcher Error {0} {1}"._DLOG(ex.Message, ex.StackTrace);

                if (_eventWatcher != null)
                    _eventWatcher.Dispose();
            }
        }

        public static List<Tuple<string, string>> GetGammaSourceAndSerial()
        {
            var vidPids = new List<string>()
            {
                @"VID_1366&PID_0105",   // SEGGER legacy driver "JLink CDC UART Port"
                @"VID_1366&PID_1024"    // WinUSB driver
            };
            Dictionary<string, List<string>> tmp = new Dictionary<string, List<string>>();
            List<Tuple<string, string>> ret = new List<Tuple<string, string>>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(
                    "root\\CIMV2",
                    @"SELECT * FROM Win32_USBControllerDevice");
            try
            {
                string lastId = "";
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    try
                    {
                        var dependent = queryObj.GetPropertyValue("Dependent") as string;
                        if (dependent != null)
                        {
                            foreach (string vidPid in vidPids)
                            {
                                if (dependent.Contains(vidPid))
                                {
                                    string id = dependent.Substring(dependent.IndexOf(vidPid) + vidPid.Length).Replace(@"\\", @"\");
                                    id = id.Trim('\\', '\"');
                                    if (!id.StartsWith("&"))
                                    {
                                        id = id.TrimStart('0');
                                        if (!tmp.ContainsKey(id))
                                        {
                                            tmp.Add(id, new List<string>());
                                            lastId = id;
                                        }
                                    }
                                    else
                                    {
                                        tmp[lastId].Add(id);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }
            catch (ManagementException) { }
            finally
            {
                searcher.Dispose();
            }

            searcher = new ManagementObjectSearcher(
                  "root\\CIMV2",
                  @"SELECT * FROM Win32_PnPEntity WHERE ClassGuid = '{4D36E978-E325-11CE-BFC1-08002BE10318}'");
            try
            {
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    var deviceId = queryObj.GetPropertyValue("DeviceID") as string;
                    if (deviceId != null)
                    {
                        foreach (string vidPid in vidPids)
                        {
                            if (deviceId.Contains(vidPid))
                            {
                                var name = queryObj.GetPropertyValue("Name") as string;
                                if (name != null)
                                {
                                    var port = name.Split('(', ')').FirstOrDefault(x => x.StartsWith("COM"));
                                    if (port != null)
                                    {
                                        foreach (var record in tmp)
                                        {
                                            foreach (var item in record.Value)
                                            {
                                                if (deviceId.Contains(item))
                                                {
                                                    ret.Add(new Tuple<string, string>(port, record.Key));
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }
                    }
                }
            }
            catch (ManagementException) { }
            finally
            {
                searcher.Dispose();
            }


            return ret;
        }

        private static void GetRelated(string indent, ManagementObject obj, string selfId)
        {
            var related = obj.GetRelated();
            foreach (ManagementObject item in related)
            {
                var itemId = item.GetPropertyValue("DeviceID") as string;
                if (selfId != itemId)
                {
                    foreach (var prop in item.Properties)
                    {
                        if (prop.Value as string != null)
                        {
                            var val = (string)prop.Value;
                            if (val.StartsWith(@"USB\VID_1366&PID_0105") || val.StartsWith(@"USB\VID_1366&PID_1024"))
                            {
                                Console.WriteLine(indent + $"{prop.Name}: {val}");
                            }
                        }
                    }
                    GetRelated(indent + "  ", item, selfId);
                }
            }
        }

        public static string[] GetDeviceNames()
        {
            var ret = SerialPort.GetPortNames();
            if (ret != null)
            {
                ret = ret.Select(x =>
                {
                    var r = x;
                    int inx = x.IndexOf('\0');
                    if (inx > 0)
                    {
                        r = x.Substring(0, inx);
                    }
                    return r;
                }).ToArray();
            }
            return ret;
        }

        private static List<Win32SerialPortClass> GetWin32SerialPortClassDevices()
        {
            return GetWin32SerialPortClassDevices(null);
        }

        private static Win32SerialPortClass GetWin32SerialPortClassDevice(string deviceId)
        {
            return GetWin32SerialPortClassDevices($"DeviceID = '{deviceId}'").FirstOrDefault();
        }

        private static List<Win32SerialPortClass> GetWin32SerialPortClassDevices(string condition)
        {
            List<Win32SerialPortClass> result = new List<Win32SerialPortClass>();
            string filter = condition?.Trim() ?? string.Empty;
            if (filter != string.Empty && !filter.ToUpper().StartsWith("AND"))
            {
                filter = " and " + filter;
            }
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_SerialPort where ConfigManagerErrorCode = 0" + filter))
            {

                foreach (var o in searcher.Get())
                {
                    var managementObject = (ManagementObject)o;
                    Win32SerialPortClass portInfo = new Win32SerialPortClass();
                    MapObject2Win32SerialPortClass(managementObject, portInfo);
                    if (!portInfo.Caption.Contains("LPT"))
                    {
                        result.Add(portInfo);
                    }
                }

            }
            return result;
        }

        public static List<Win32PnPEntityClass> GetWin32PnPEntityClassSerialPortDevices()
        {
            return GetWin32PnPEntityClassSerialPortDevices(null);
        }

        public static Win32PnPEntityClass GetWin32PnPEntityClassSerialPortDevice(string serialPortName)
        {
            return GetWin32PnPEntityClassSerialPortDevices($"Name like '%({serialPortName})%'").FirstOrDefault();
        }

        public static List<Win32PnPEntityClass> GetWin32PnPEntityClassSerialPortDevices(string condition)
        {
            List<Win32PnPEntityClass> result = new List<Win32PnPEntityClass>();
            string filter = condition?.Trim() ?? string.Empty;
            if (filter != string.Empty && !filter.ToUpper().StartsWith("AND"))
            {
                filter = " and " + filter;
            }
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where ClassGuid = '{4D36E978-E325-11CE-BFC1-08002BE10318}' and ConfigManagerErrorCode = 0" + filter))
            {
                try
                {
                    foreach (var o in searcher.Get())
                    {
                        var managementObject = (ManagementObject)o;
                        Win32PnPEntityClass portInfo = new Win32PnPEntityClass();
                        MapObject2Win32PnPEntityClass(managementObject, portInfo);
                        if (!portInfo.Caption.Contains("LPT"))
                        {
                            result.Add(portInfo);
                        }
                    }
                }
                catch (Exception ex)
                {
                    "GetWin32PnPEntityClassSerialPortDevices Error {0} {1}"._DLOG(ex.Message, ex.StackTrace);
                }
                return result;
            }
        }

        private static List<Win32PnPEntityClass> GetWin32PnPEntityClassDevices()
        {
            return GetWin32PnPEntityClassDevices(null);
        }

        private static List<Win32PnPEntityClass> GetWin32PnPEntityClassDevices(string condition)
        {
            List<Win32PnPEntityClass> result = new List<Win32PnPEntityClass>();
            string filter = condition?.Trim() ?? string.Empty;
            if (filter != string.Empty && !filter.ToUpper().StartsWith("AND"))
            {
                filter = " and " + filter;
            }
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from Win32_PnPEntity where ConfigManagerErrorCode = 0" + filter))
            {
                foreach (var o in searcher.Get())
                {
                    var managementObject = (ManagementObject)o;
                    Win32PnPEntityClass portInfo = new Win32PnPEntityClass();
                    MapObject2Win32PnPEntityClass(managementObject, portInfo);
                    if (!portInfo.Caption.Contains("LPT"))
                    {
                        result.Add(portInfo);
                    }
                }
            }
            return result;
        }

        public static void KillProcessWhereCommandLineLike(string filter)
        {
            using (var mos = new ManagementObjectSearcher($"SELECT ProcessId FROM Win32_Process WHERE commandline like '{filter}'"))
            {
                foreach (ManagementObject mo in mos.Get())
                {
                    var id = mo["ProcessId"];
                    if (id != null && id is uint)
                    {
                        var process = Process.GetProcessById((int)(uint)id);
                        if (process != null)
                        {
                            process.Kill();
                        }
                    }
                }

            }
        }
        #endregion

        #region Private Methods
        private static void MapObject2BaseWin32DeviceClass(ManagementBaseObject managementObject, BaseWin32DeviceClass device)
        {
            try
            {
                device.HardwareId = "";// managementObject.Properties["HardwareID"].Value;
                object hIdValue = FindPropertyValue(managementObject.Properties, "HardwareID");
                if (hIdValue != null)
                {
                    var id = hIdValue as string[];
                    if (id != null)
                    {
                        string[] hardwareId = id;
                        if (hardwareId.Length >= 1)
                        {
                            if (hardwareId[0].ToUpper().Contains("REV_"))
                            {
                                device.HardwareId = hardwareId[0].ToUpper().Substring(hardwareId[0].ToUpper().IndexOf("REV_", StringComparison.Ordinal) + 4);
                            }
                        }
                    }
                }
            }
            catch
            {
                // ignored
            }

            if (managementObject.Properties["Availability"].Value != null)
                device.Availability = (ushort?)managementObject.Properties["Availability"].Value;

            if (managementObject.Properties["Caption"].Value != null)
            {
                device.Caption = (string)managementObject.Properties["Caption"].Value;
            }

            if (managementObject.Properties["ConfigManagerErrorCode"].Value != null)
                device.ConfigManagerErrorCode = (uint?)managementObject.Properties["ConfigManagerErrorCode"].Value;

            if (managementObject.Properties["ConfigManagerUserConfig"].Value != null)
                device.ConfigManagerUserConfig = (bool?)managementObject.Properties["ConfigManagerUserConfig"].Value;

            if (managementObject.Properties["CreationClassName"].Value != null)
                device.CreationClassName = (string)managementObject.Properties["CreationClassName"].Value;

            if (managementObject.Properties["Description"].Value != null)
                device.Description = (string)managementObject.Properties["Description"].Value;

            if (managementObject.Properties["DeviceID"].Value != null)
                device.DeviceId = (string)managementObject.Properties["DeviceID"].Value;

            if (managementObject.Properties["Name"].Value != null)
                device.Name = (string)managementObject.Properties["Name"].Value;

            if (managementObject.Properties["PNPDeviceID"].Value != null)
                device.PnpDeviceId = (string)managementObject.Properties["PNPDeviceID"].Value;

            if (managementObject.Properties["PowerManagementCapabilities"].Value != null)
                device.PowerManagementCapabilities = (ushort[])managementObject.Properties["PowerManagementCapabilities"].Value;

            if (managementObject.Properties["PowerManagementSupported"] != null)
            {
                if (managementObject.Properties["PowerManagementSupported"].Value != null)
                    device.PowerManagementSupported = (bool?)managementObject.Properties["PowerManagementSupported"].Value;
            }

            if (managementObject.Properties["Status"] != null)
            {
                if (managementObject.Properties["Status"].Value != null)
                    device.Status = (string)managementObject.Properties["Status"].Value;
            }

            if (managementObject.Properties["StatusInfo"] != null)
            {
                if (managementObject.Properties["StatusInfo"].Value != null)
                    device.StatusInfo = (ushort?)managementObject.Properties["StatusInfo"].Value;
            }

            if (managementObject.Properties["SystemCreationClassName"] != null)
            {
                if (managementObject.Properties["SystemCreationClassName"].Value != null)
                    device.SystemCreationClassName = (string)managementObject.Properties["SystemCreationClassName"].Value;
            }

            if (managementObject.Properties["SystemName"] != null)
            {
                if (managementObject.Properties["SystemName"].Value != null)
                    device.SystemName = (string)managementObject.Properties["SystemName"].Value;
            }

            if (string.IsNullOrEmpty(device.HardwareId) && device.DeviceId.ToUpper().Contains("Vid_0658&Pid_0280".ToUpper()))
            {
                try
                {
                    object hId = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\ControlSet001\Enum\USB\Vid_0658&Pid_0280\5&25039244&0&1", "HardwareID", null);
                    if (hId != null)
                    {
                        var id = hId as string[];
                        if (id != null)
                        {
                            string[] hardwareId = id;
                            if (hardwareId.Length >= 1)
                            {
                                if (hardwareId[0].ToUpper().Contains("REV_"))
                                {
                                    device.HardwareId = hardwareId[0].ToUpper().Substring(hardwareId[0].ToUpper().IndexOf("REV_", StringComparison.Ordinal) + 4);
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }

        private static object FindPropertyValue(IEnumerable propertyDataCollection, string propertyName)
        {
            return (from PropertyData pData in propertyDataCollection where string.Equals(pData.Name, propertyName, StringComparison.CurrentCultureIgnoreCase) select pData.Value).FirstOrDefault();
        }

        private static void MapObject2Win32SerialPortClass(ManagementBaseObject managementObject, Win32SerialPortClass portInfo)
        {
            MapObject2BaseWin32DeviceClass(managementObject, portInfo);
            if (managementObject.Properties["Binary"].Value != null)
                portInfo.Binary = (bool?)managementObject.Properties["Binary"].Value;

            if (managementObject.Properties["MaxBaudRate"].Value != null)
                portInfo.MaxBaudRate = (uint?)managementObject.Properties["MaxBaudRate"].Value;

            if (managementObject.Properties["MaximumInputBufferSize"].Value != null)
                portInfo.MaximumInputBufferSize = (uint?)managementObject.Properties["MaximumInputBufferSize"].Value;

            if (managementObject.Properties["MaximumOutputBufferSize"].Value != null)
                portInfo.MaximumOutputBufferSize = (uint?)managementObject.Properties["MaximumOutputBufferSize"].Value;

            if (managementObject.Properties["OSAutoDiscovered"].Value != null)
                portInfo.OsAutoDiscovered = (bool?)managementObject.Properties["OSAutoDiscovered"].Value;

            if (managementObject.Properties["ProviderType"].Value != null)
                portInfo.ProviderType = (string)managementObject.Properties["ProviderType"].Value;

            if (managementObject.Properties["SettableBaudRate"].Value != null)
                portInfo.SettableBaudRate = (bool?)managementObject.Properties["SettableBaudRate"].Value;

            if (managementObject.Properties["SettableDataBits"].Value != null)
                portInfo.SettableDataBits = (bool?)managementObject.Properties["SettableDataBits"].Value;

            if (managementObject.Properties["SettableFlowControl"].Value != null)
                portInfo.SettableFlowControl = (bool?)managementObject.Properties["SettableFlowControl"].Value;

            if (managementObject.Properties["SettableParity"].Value != null)
                portInfo.SettableParity = (bool?)managementObject.Properties["SettableParity"].Value;

            if (managementObject.Properties["SettableParityCheck"].Value != null)
                portInfo.SettableParityCheck = (bool?)managementObject.Properties["SettableParityCheck"].Value;

            if (managementObject.Properties["SettableRLSD"] != null)
            {
                if (managementObject.Properties["SettableRLSD"].Value != null)
                    portInfo.SettableRlsd = (bool?)managementObject.Properties["SettableRLSD"].Value;
            }

            if (managementObject.Properties["SettableStopBits"] != null)
            {
                if (managementObject.Properties["SettableStopBits"].Value != null)
                    portInfo.SettableStopBits = (bool?)managementObject.Properties["SettableStopBits"].Value;
            }

            if (managementObject.Properties["Supports16BitMode"] != null)
            {
                if (managementObject.Properties["Supports16BitMode"].Value != null)
                    portInfo.Supports16BitMode = (bool?)managementObject.Properties["Supports16BitMode"].Value;
            }

            if (managementObject.Properties["SupportsDTRDSR"] != null)
            {
                if (managementObject.Properties["SupportsDTRDSR"].Value != null)
                    portInfo.SupportsDtrdsr = (bool?)managementObject.Properties["SupportsDTRDSR"].Value;
            }

            if (managementObject.Properties["SupportsElapsedTimeouts"] != null)
            {
                if (managementObject.Properties["SupportsElapsedTimeouts"].Value != null)
                    portInfo.SupportsElapsedTimeouts = (bool?)managementObject.Properties["SupportsElapsedTimeouts"].Value;
            }

            if (managementObject.Properties["SupportsIntTimeouts"] != null)
            {
                if (managementObject.Properties["SupportsIntTimeouts"].Value != null)
                    portInfo.SupportsIntTimeouts = (bool?)managementObject.Properties["SupportsIntTimeouts"].Value;
            }

            if (managementObject.Properties["SupportsParityCheck"] != null)
            {
                if (managementObject.Properties["SupportsParityCheck"].Value != null)
                    portInfo.SupportsParityCheck = (bool?)managementObject.Properties["SupportsParityCheck"].Value;
            }

            if (managementObject.Properties["SupportsRLSD"] != null)
            {
                if (managementObject.Properties["SupportsRLSD"].Value != null)
                    portInfo.SupportsRlsd = (bool?)managementObject.Properties["SupportsRLSD"].Value;
            }

            if (managementObject.Properties["SupportsRTSCTS"] != null)
            {
                if (managementObject.Properties["SupportsRTSCTS"].Value != null)
                    portInfo.SupportsRtscts = (bool?)managementObject.Properties["SupportsRTSCTS"].Value;
            }

            if (managementObject.Properties["SupportsSpecialCharacters"] != null)
            {
                if (managementObject.Properties["SupportsSpecialCharacters"].Value != null)
                    portInfo.SupportsSpecialCharacters = (bool?)managementObject.Properties["SupportsSpecialCharacters"].Value;
            }

            if (managementObject.Properties["SupportsXOnXOff"] != null)
            {
                if (managementObject.Properties["SupportsXOnXOff"].Value != null)
                    portInfo.SupportsXonXOff = (bool?)managementObject.Properties["SupportsXOnXOff"].Value;
            }

            if (managementObject.Properties["SupportsXOnXOffSet"] != null)
            {
                if (managementObject.Properties["SupportsXOnXOffSet"].Value != null)
                    portInfo.SupportsXonXOffSet = (bool?)managementObject.Properties["SupportsXOnXOffSet"].Value;
            }
        }
        private static void MapObject2Win32PnPEntityClass(ManagementBaseObject managementObject, Win32PnPEntityClass portInfo)
        {
            MapObject2BaseWin32DeviceClass(managementObject, portInfo);
            if (managementObject.Properties["ClassGuid"].Value != null)
                portInfo.ClassGuid = (string)managementObject.Properties["ClassGuid"].Value;
            if (managementObject.Properties["ErrorCleared"].Value != null)
                portInfo.ErrorCleared = (bool?)managementObject.Properties["ErrorCleared"].Value;
            if (managementObject.Properties["ErrorDescription"].Value != null)
                portInfo.ErrorDescription = (string)managementObject.Properties["ErrorDescription"].Value;
            if (managementObject.Properties["InstallDate"].Value != null)
                portInfo.InstallDate = (DateTime?)managementObject.Properties["InstallDate"].Value;
            if (managementObject.Properties["LastErrorCode"].Value != null)
                portInfo.LastErrorCode = (int?)managementObject.Properties["LastErrorCode"].Value;
            if (managementObject.Properties["Manufacturer"].Value != null)
                portInfo.Manufacturer = (string)managementObject.Properties["Manufacturer"].Value;
            if (managementObject.Properties["Service"].Value != null)
                portInfo.Service = (string)managementObject.Properties["Service"].Value;

            var match = Regex.Match(portInfo.Name, @"\((COM\d*)\)");
            if (match.Success)
            {
                portInfo.SerialPortName = match.Value.Trim('(', ')');
            }
        }
        #endregion
    }
}

