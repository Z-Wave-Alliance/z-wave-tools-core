// SPDX-License-Identifier: BSD-3-Clause
// SPDX-FileCopyrightText: Z-Wave Alliance <https://z-wavealliance.org>
using System.Collections.Generic;

namespace ZWave.CommandClasses
{
    public partial class COMMAND_CLASS_GENERAL_DIAGNOSTICS
    {
        public const byte ID = 0xA5;
        public const byte VERSION = 1;
        public partial class NETWORK_DIAGNOSTICS_GET
        {
            public const byte ID = 0x01;
            public static implicit operator NETWORK_DIAGNOSTICS_GET(byte[] data)
            {
                NETWORK_DIAGNOSTICS_GET ret = new NETWORK_DIAGNOSTICS_GET();
                return ret;
            }
            public static implicit operator byte[](NETWORK_DIAGNOSTICS_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERAL_DIAGNOSTICS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class NETWORK_DIAGNOSTICS_REPORT
        {
            public const byte ID = 0x02;
            public const byte timeSinceDataCollectionStartBytesCount = 4;
            public byte[] timeSinceDataCollectionStart = new byte[timeSinceDataCollectionStartBytesCount];
            public const byte packetsReceivedCountBytesCount = 2;
            public byte[] packetsReceivedCount = new byte[packetsReceivedCountBytesCount];
            public const byte packetsSubmittedForTxCountBytesCount = 2;
            public byte[] packetsSubmittedForTxCount = new byte[packetsSubmittedForTxCountBytesCount];
            public const byte packetsTxAcceptedCountBytesCount = 2;
            public byte[] packetsTxAcceptedCount = new byte[packetsTxAcceptedCountBytesCount];
            public const byte txCompleteOkCountBytesCount = 2;
            public byte[] txCompleteOkCount = new byte[txCompleteOkCountBytesCount];
            public const byte txCompleteNoAckCountBytesCount = 2;
            public byte[] txCompleteNoAckCount = new byte[txCompleteNoAckCountBytesCount];
            public const byte txCompleteFailCountBytesCount = 2;
            public byte[] txCompleteFailCount = new byte[txCompleteFailCountBytesCount];
            public const byte txCompleteNoRouteCountBytesCount = 2;
            public byte[] txCompleteNoRouteCount = new byte[txCompleteNoRouteCountBytesCount];
            public const byte explorerFrameTxCountBytesCount = 2;
            public byte[] explorerFrameTxCount = new byte[explorerFrameTxCountBytesCount];
            public const byte txRouteChangeCountBytesCount = 2;
            public byte[] txRouteChangeCount = new byte[txRouteChangeCountBytesCount];
            public const byte dynamicTxPowerResetCountBytesCount = 2;
            public byte[] dynamicTxPowerResetCount = new byte[dynamicTxPowerResetCountBytesCount];
            public const byte rxRssiBin1CountBytesCount = 2;
            public byte[] rxRssiBin1Count = new byte[rxRssiBin1CountBytesCount];
            public const byte rxRssiBin2CountBytesCount = 2;
            public byte[] rxRssiBin2Count = new byte[rxRssiBin2CountBytesCount];
            public const byte rxRssiBin3CountBytesCount = 2;
            public byte[] rxRssiBin3Count = new byte[rxRssiBin3CountBytesCount];
            public const byte rxRssiBin4CountBytesCount = 2;
            public byte[] rxRssiBin4Count = new byte[rxRssiBin4CountBytesCount];
            public const byte rxRssiBin5CountBytesCount = 2;
            public byte[] rxRssiBin5Count = new byte[rxRssiBin5CountBytesCount];
            public const byte txPowerBin1CountBytesCount = 2;
            public byte[] txPowerBin1Count = new byte[txPowerBin1CountBytesCount];
            public const byte txPowerBin2CountBytesCount = 2;
            public byte[] txPowerBin2Count = new byte[txPowerBin2CountBytesCount];
            public const byte txPowerBin3CountBytesCount = 2;
            public byte[] txPowerBin3Count = new byte[txPowerBin3CountBytesCount];
            public const byte txPowerBin4CountBytesCount = 2;
            public byte[] txPowerBin4Count = new byte[txPowerBin4CountBytesCount];
            public const byte txPowerBin5CountBytesCount = 2;
            public byte[] txPowerBin5Count = new byte[txPowerBin5CountBytesCount];
            public const byte noiseFloorRxBin1CountBytesCount = 2;
            public byte[] noiseFloorRxBin1Count = new byte[noiseFloorRxBin1CountBytesCount];
            public const byte noiseFloorRxBin2CountBytesCount = 2;
            public byte[] noiseFloorRxBin2Count = new byte[noiseFloorRxBin2CountBytesCount];
            public const byte noiseFloorRxBin3CountBytesCount = 2;
            public byte[] noiseFloorRxBin3Count = new byte[noiseFloorRxBin3CountBytesCount];
            public const byte noiseFloorRxBin4CountBytesCount = 2;
            public byte[] noiseFloorRxBin4Count = new byte[noiseFloorRxBin4CountBytesCount];
            public const byte noiseFloorRxBin5CountBytesCount = 2;
            public byte[] noiseFloorRxBin5Count = new byte[noiseFloorRxBin5CountBytesCount];
            public const byte noiseFloorTxBin1CountBytesCount = 2;
            public byte[] noiseFloorTxBin1Count = new byte[noiseFloorTxBin1CountBytesCount];
            public const byte noiseFloorTxBin2CountBytesCount = 2;
            public byte[] noiseFloorTxBin2Count = new byte[noiseFloorTxBin2CountBytesCount];
            public const byte noiseFloorTxBin3CountBytesCount = 2;
            public byte[] noiseFloorTxBin3Count = new byte[noiseFloorTxBin3CountBytesCount];
            public const byte noiseFloorTxBin4CountBytesCount = 2;
            public byte[] noiseFloorTxBin4Count = new byte[noiseFloorTxBin4CountBytesCount];
            public const byte noiseFloorTxBin5CountBytesCount = 2;
            public byte[] noiseFloorTxBin5Count = new byte[noiseFloorTxBin5CountBytesCount];
            public const byte channelChangeCountBytesCount = 2;
            public byte[] channelChangeCount = new byte[channelChangeCountBytesCount];
            public static implicit operator NETWORK_DIAGNOSTICS_REPORT(byte[] data)
            {
                NETWORK_DIAGNOSTICS_REPORT ret = new NETWORK_DIAGNOSTICS_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.timeSinceDataCollectionStart = (data.Length - index) >= timeSinceDataCollectionStartBytesCount ? new byte[timeSinceDataCollectionStartBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.timeSinceDataCollectionStart[0] = data[index++];
                    if (data.Length > index) ret.timeSinceDataCollectionStart[1] = data[index++];
                    if (data.Length > index) ret.timeSinceDataCollectionStart[2] = data[index++];
                    if (data.Length > index) ret.timeSinceDataCollectionStart[3] = data[index++];
                    ret.packetsReceivedCount = (data.Length - index) >= packetsReceivedCountBytesCount ? new byte[packetsReceivedCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.packetsReceivedCount[0] = data[index++];
                    if (data.Length > index) ret.packetsReceivedCount[1] = data[index++];
                    ret.packetsSubmittedForTxCount = (data.Length - index) >= packetsSubmittedForTxCountBytesCount ? new byte[packetsSubmittedForTxCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.packetsSubmittedForTxCount[0] = data[index++];
                    if (data.Length > index) ret.packetsSubmittedForTxCount[1] = data[index++];
                    ret.packetsTxAcceptedCount = (data.Length - index) >= packetsTxAcceptedCountBytesCount ? new byte[packetsTxAcceptedCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.packetsTxAcceptedCount[0] = data[index++];
                    if (data.Length > index) ret.packetsTxAcceptedCount[1] = data[index++];
                    ret.txCompleteOkCount = (data.Length - index) >= txCompleteOkCountBytesCount ? new byte[txCompleteOkCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txCompleteOkCount[0] = data[index++];
                    if (data.Length > index) ret.txCompleteOkCount[1] = data[index++];
                    ret.txCompleteNoAckCount = (data.Length - index) >= txCompleteNoAckCountBytesCount ? new byte[txCompleteNoAckCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txCompleteNoAckCount[0] = data[index++];
                    if (data.Length > index) ret.txCompleteNoAckCount[1] = data[index++];
                    ret.txCompleteFailCount = (data.Length - index) >= txCompleteFailCountBytesCount ? new byte[txCompleteFailCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txCompleteFailCount[0] = data[index++];
                    if (data.Length > index) ret.txCompleteFailCount[1] = data[index++];
                    ret.txCompleteNoRouteCount = (data.Length - index) >= txCompleteNoRouteCountBytesCount ? new byte[txCompleteNoRouteCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txCompleteNoRouteCount[0] = data[index++];
                    if (data.Length > index) ret.txCompleteNoRouteCount[1] = data[index++];
                    ret.explorerFrameTxCount = (data.Length - index) >= explorerFrameTxCountBytesCount ? new byte[explorerFrameTxCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.explorerFrameTxCount[0] = data[index++];
                    if (data.Length > index) ret.explorerFrameTxCount[1] = data[index++];
                    ret.txRouteChangeCount = (data.Length - index) >= txRouteChangeCountBytesCount ? new byte[txRouteChangeCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txRouteChangeCount[0] = data[index++];
                    if (data.Length > index) ret.txRouteChangeCount[1] = data[index++];
                    ret.dynamicTxPowerResetCount = (data.Length - index) >= dynamicTxPowerResetCountBytesCount ? new byte[dynamicTxPowerResetCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.dynamicTxPowerResetCount[0] = data[index++];
                    if (data.Length > index) ret.dynamicTxPowerResetCount[1] = data[index++];
                    ret.rxRssiBin1Count = (data.Length - index) >= rxRssiBin1CountBytesCount ? new byte[rxRssiBin1CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.rxRssiBin1Count[0] = data[index++];
                    if (data.Length > index) ret.rxRssiBin1Count[1] = data[index++];
                    ret.rxRssiBin2Count = (data.Length - index) >= rxRssiBin2CountBytesCount ? new byte[rxRssiBin2CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.rxRssiBin2Count[0] = data[index++];
                    if (data.Length > index) ret.rxRssiBin2Count[1] = data[index++];
                    ret.rxRssiBin3Count = (data.Length - index) >= rxRssiBin3CountBytesCount ? new byte[rxRssiBin3CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.rxRssiBin3Count[0] = data[index++];
                    if (data.Length > index) ret.rxRssiBin3Count[1] = data[index++];
                    ret.rxRssiBin4Count = (data.Length - index) >= rxRssiBin4CountBytesCount ? new byte[rxRssiBin4CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.rxRssiBin4Count[0] = data[index++];
                    if (data.Length > index) ret.rxRssiBin4Count[1] = data[index++];
                    ret.rxRssiBin5Count = (data.Length - index) >= rxRssiBin5CountBytesCount ? new byte[rxRssiBin5CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.rxRssiBin5Count[0] = data[index++];
                    if (data.Length > index) ret.rxRssiBin5Count[1] = data[index++];
                    ret.txPowerBin1Count = (data.Length - index) >= txPowerBin1CountBytesCount ? new byte[txPowerBin1CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txPowerBin1Count[0] = data[index++];
                    if (data.Length > index) ret.txPowerBin1Count[1] = data[index++];
                    ret.txPowerBin2Count = (data.Length - index) >= txPowerBin2CountBytesCount ? new byte[txPowerBin2CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txPowerBin2Count[0] = data[index++];
                    if (data.Length > index) ret.txPowerBin2Count[1] = data[index++];
                    ret.txPowerBin3Count = (data.Length - index) >= txPowerBin3CountBytesCount ? new byte[txPowerBin3CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txPowerBin3Count[0] = data[index++];
                    if (data.Length > index) ret.txPowerBin3Count[1] = data[index++];
                    ret.txPowerBin4Count = (data.Length - index) >= txPowerBin4CountBytesCount ? new byte[txPowerBin4CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txPowerBin4Count[0] = data[index++];
                    if (data.Length > index) ret.txPowerBin4Count[1] = data[index++];
                    ret.txPowerBin5Count = (data.Length - index) >= txPowerBin5CountBytesCount ? new byte[txPowerBin5CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.txPowerBin5Count[0] = data[index++];
                    if (data.Length > index) ret.txPowerBin5Count[1] = data[index++];
                    ret.noiseFloorRxBin1Count = (data.Length - index) >= noiseFloorRxBin1CountBytesCount ? new byte[noiseFloorRxBin1CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorRxBin1Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorRxBin1Count[1] = data[index++];
                    ret.noiseFloorRxBin2Count = (data.Length - index) >= noiseFloorRxBin2CountBytesCount ? new byte[noiseFloorRxBin2CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorRxBin2Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorRxBin2Count[1] = data[index++];
                    ret.noiseFloorRxBin3Count = (data.Length - index) >= noiseFloorRxBin3CountBytesCount ? new byte[noiseFloorRxBin3CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorRxBin3Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorRxBin3Count[1] = data[index++];
                    ret.noiseFloorRxBin4Count = (data.Length - index) >= noiseFloorRxBin4CountBytesCount ? new byte[noiseFloorRxBin4CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorRxBin4Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorRxBin4Count[1] = data[index++];
                    ret.noiseFloorRxBin5Count = (data.Length - index) >= noiseFloorRxBin5CountBytesCount ? new byte[noiseFloorRxBin5CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorRxBin5Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorRxBin5Count[1] = data[index++];
                    ret.noiseFloorTxBin1Count = (data.Length - index) >= noiseFloorTxBin1CountBytesCount ? new byte[noiseFloorTxBin1CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorTxBin1Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorTxBin1Count[1] = data[index++];
                    ret.noiseFloorTxBin2Count = (data.Length - index) >= noiseFloorTxBin2CountBytesCount ? new byte[noiseFloorTxBin2CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorTxBin2Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorTxBin2Count[1] = data[index++];
                    ret.noiseFloorTxBin3Count = (data.Length - index) >= noiseFloorTxBin3CountBytesCount ? new byte[noiseFloorTxBin3CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorTxBin3Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorTxBin3Count[1] = data[index++];
                    ret.noiseFloorTxBin4Count = (data.Length - index) >= noiseFloorTxBin4CountBytesCount ? new byte[noiseFloorTxBin4CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorTxBin4Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorTxBin4Count[1] = data[index++];
                    ret.noiseFloorTxBin5Count = (data.Length - index) >= noiseFloorTxBin5CountBytesCount ? new byte[noiseFloorTxBin5CountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.noiseFloorTxBin5Count[0] = data[index++];
                    if (data.Length > index) ret.noiseFloorTxBin5Count[1] = data[index++];
                    ret.channelChangeCount = (data.Length - index) >= channelChangeCountBytesCount ? new byte[channelChangeCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.channelChangeCount[0] = data[index++];
                    if (data.Length > index) ret.channelChangeCount[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](NETWORK_DIAGNOSTICS_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERAL_DIAGNOSTICS.ID);
                ret.Add(ID);
                if (command.timeSinceDataCollectionStart != null)
                {
                    foreach (var tmp in command.timeSinceDataCollectionStart)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.packetsReceivedCount != null)
                {
                    foreach (var tmp in command.packetsReceivedCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.packetsSubmittedForTxCount != null)
                {
                    foreach (var tmp in command.packetsSubmittedForTxCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.packetsTxAcceptedCount != null)
                {
                    foreach (var tmp in command.packetsTxAcceptedCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txCompleteOkCount != null)
                {
                    foreach (var tmp in command.txCompleteOkCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txCompleteNoAckCount != null)
                {
                    foreach (var tmp in command.txCompleteNoAckCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txCompleteFailCount != null)
                {
                    foreach (var tmp in command.txCompleteFailCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txCompleteNoRouteCount != null)
                {
                    foreach (var tmp in command.txCompleteNoRouteCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.explorerFrameTxCount != null)
                {
                    foreach (var tmp in command.explorerFrameTxCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txRouteChangeCount != null)
                {
                    foreach (var tmp in command.txRouteChangeCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.dynamicTxPowerResetCount != null)
                {
                    foreach (var tmp in command.dynamicTxPowerResetCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.rxRssiBin1Count != null)
                {
                    foreach (var tmp in command.rxRssiBin1Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.rxRssiBin2Count != null)
                {
                    foreach (var tmp in command.rxRssiBin2Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.rxRssiBin3Count != null)
                {
                    foreach (var tmp in command.rxRssiBin3Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.rxRssiBin4Count != null)
                {
                    foreach (var tmp in command.rxRssiBin4Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.rxRssiBin5Count != null)
                {
                    foreach (var tmp in command.rxRssiBin5Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txPowerBin1Count != null)
                {
                    foreach (var tmp in command.txPowerBin1Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txPowerBin2Count != null)
                {
                    foreach (var tmp in command.txPowerBin2Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txPowerBin3Count != null)
                {
                    foreach (var tmp in command.txPowerBin3Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txPowerBin4Count != null)
                {
                    foreach (var tmp in command.txPowerBin4Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.txPowerBin5Count != null)
                {
                    foreach (var tmp in command.txPowerBin5Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorRxBin1Count != null)
                {
                    foreach (var tmp in command.noiseFloorRxBin1Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorRxBin2Count != null)
                {
                    foreach (var tmp in command.noiseFloorRxBin2Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorRxBin3Count != null)
                {
                    foreach (var tmp in command.noiseFloorRxBin3Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorRxBin4Count != null)
                {
                    foreach (var tmp in command.noiseFloorRxBin4Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorRxBin5Count != null)
                {
                    foreach (var tmp in command.noiseFloorRxBin5Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorTxBin1Count != null)
                {
                    foreach (var tmp in command.noiseFloorTxBin1Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorTxBin2Count != null)
                {
                    foreach (var tmp in command.noiseFloorTxBin2Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorTxBin3Count != null)
                {
                    foreach (var tmp in command.noiseFloorTxBin3Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorTxBin4Count != null)
                {
                    foreach (var tmp in command.noiseFloorTxBin4Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.noiseFloorTxBin5Count != null)
                {
                    foreach (var tmp in command.noiseFloorTxBin5Count)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.channelChangeCount != null)
                {
                    foreach (var tmp in command.channelChangeCount)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class NETWORK_DIAGNOSTICS_RESET
        {
            public const byte ID = 0x03;
            public static implicit operator NETWORK_DIAGNOSTICS_RESET(byte[] data)
            {
                NETWORK_DIAGNOSTICS_RESET ret = new NETWORK_DIAGNOSTICS_RESET();
                return ret;
            }
            public static implicit operator byte[](NETWORK_DIAGNOSTICS_RESET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERAL_DIAGNOSTICS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BOOT_TRACKING_GET
        {
            public const byte ID = 0x04;
            public static implicit operator BOOT_TRACKING_GET(byte[] data)
            {
                BOOT_TRACKING_GET ret = new BOOT_TRACKING_GET();
                return ret;
            }
            public static implicit operator byte[](BOOT_TRACKING_GET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERAL_DIAGNOSTICS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
        public partial class BOOT_TRACKING_REPORT
        {
            public const byte ID = 0x05;
            public const byte unspecifiedBootCountBytesCount = 2;
            public byte[] unspecifiedBootCount = new byte[unspecifiedBootCountBytesCount];
            public const byte poweronBootCountBytesCount = 2;
            public byte[] poweronBootCount = new byte[poweronBootCountBytesCount];
            public const byte brownoutBootCountBytesCount = 2;
            public byte[] brownoutBootCount = new byte[brownoutBootCountBytesCount];
            public const byte watchdogBootCountBytesCount = 2;
            public byte[] watchdogBootCount = new byte[watchdogBootCountBytesCount];
            public const byte hardwareresetBootCountBytesCount = 2;
            public byte[] hardwareresetBootCount = new byte[hardwareresetBootCountBytesCount];
            public const byte softwareupdatesuccessBootCountBytesCount = 2;
            public byte[] softwareupdatesuccessBootCount = new byte[softwareupdatesuccessBootCountBytesCount];
            public const byte softwareupdatefailedBootCountBytesCount = 2;
            public byte[] softwareupdatefailedBootCount = new byte[softwareupdatefailedBootCountBytesCount];
            public const byte networkexcludedBootCountBytesCount = 2;
            public byte[] networkexcludedBootCount = new byte[networkexcludedBootCountBytesCount];
            public const byte regionconfigurationBootCountBytesCount = 2;
            public byte[] regionconfigurationBootCount = new byte[regionconfigurationBootCountBytesCount];
            public const byte nvmcorruptionBootCountBytesCount = 2;
            public byte[] nvmcorruptionBootCount = new byte[nvmcorruptionBootCountBytesCount];
            public const byte softwareassertBootCountBytesCount = 2;
            public byte[] softwareassertBootCount = new byte[softwareassertBootCountBytesCount];
            public const byte factoryresetBootCountBytesCount = 2;
            public byte[] factoryresetBootCount = new byte[factoryresetBootCountBytesCount];
            public const byte stackoverflowBootCountBytesCount = 2;
            public byte[] stackoverflowBootCount = new byte[stackoverflowBootCountBytesCount];
            public static implicit operator BOOT_TRACKING_REPORT(byte[] data)
            {
                BOOT_TRACKING_REPORT ret = new BOOT_TRACKING_REPORT();
                if (data != null)
                {
                    int index = 2;
                    ret.unspecifiedBootCount = (data.Length - index) >= unspecifiedBootCountBytesCount ? new byte[unspecifiedBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.unspecifiedBootCount[0] = data[index++];
                    if (data.Length > index) ret.unspecifiedBootCount[1] = data[index++];
                    ret.poweronBootCount = (data.Length - index) >= poweronBootCountBytesCount ? new byte[poweronBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.poweronBootCount[0] = data[index++];
                    if (data.Length > index) ret.poweronBootCount[1] = data[index++];
                    ret.brownoutBootCount = (data.Length - index) >= brownoutBootCountBytesCount ? new byte[brownoutBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.brownoutBootCount[0] = data[index++];
                    if (data.Length > index) ret.brownoutBootCount[1] = data[index++];
                    ret.watchdogBootCount = (data.Length - index) >= watchdogBootCountBytesCount ? new byte[watchdogBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.watchdogBootCount[0] = data[index++];
                    if (data.Length > index) ret.watchdogBootCount[1] = data[index++];
                    ret.hardwareresetBootCount = (data.Length - index) >= hardwareresetBootCountBytesCount ? new byte[hardwareresetBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.hardwareresetBootCount[0] = data[index++];
                    if (data.Length > index) ret.hardwareresetBootCount[1] = data[index++];
                    ret.softwareupdatesuccessBootCount = (data.Length - index) >= softwareupdatesuccessBootCountBytesCount ? new byte[softwareupdatesuccessBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.softwareupdatesuccessBootCount[0] = data[index++];
                    if (data.Length > index) ret.softwareupdatesuccessBootCount[1] = data[index++];
                    ret.softwareupdatefailedBootCount = (data.Length - index) >= softwareupdatefailedBootCountBytesCount ? new byte[softwareupdatefailedBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.softwareupdatefailedBootCount[0] = data[index++];
                    if (data.Length > index) ret.softwareupdatefailedBootCount[1] = data[index++];
                    ret.networkexcludedBootCount = (data.Length - index) >= networkexcludedBootCountBytesCount ? new byte[networkexcludedBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.networkexcludedBootCount[0] = data[index++];
                    if (data.Length > index) ret.networkexcludedBootCount[1] = data[index++];
                    ret.regionconfigurationBootCount = (data.Length - index) >= regionconfigurationBootCountBytesCount ? new byte[regionconfigurationBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.regionconfigurationBootCount[0] = data[index++];
                    if (data.Length > index) ret.regionconfigurationBootCount[1] = data[index++];
                    ret.nvmcorruptionBootCount = (data.Length - index) >= nvmcorruptionBootCountBytesCount ? new byte[nvmcorruptionBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.nvmcorruptionBootCount[0] = data[index++];
                    if (data.Length > index) ret.nvmcorruptionBootCount[1] = data[index++];
                    ret.softwareassertBootCount = (data.Length - index) >= softwareassertBootCountBytesCount ? new byte[softwareassertBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.softwareassertBootCount[0] = data[index++];
                    if (data.Length > index) ret.softwareassertBootCount[1] = data[index++];
                    ret.factoryresetBootCount = (data.Length - index) >= factoryresetBootCountBytesCount ? new byte[factoryresetBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.factoryresetBootCount[0] = data[index++];
                    if (data.Length > index) ret.factoryresetBootCount[1] = data[index++];
                    ret.stackoverflowBootCount = (data.Length - index) >= stackoverflowBootCountBytesCount ? new byte[stackoverflowBootCountBytesCount] : new byte[data.Length - index];
                    if (data.Length > index) ret.stackoverflowBootCount[0] = data[index++];
                    if (data.Length > index) ret.stackoverflowBootCount[1] = data[index++];
                }
                return ret;
            }
            public static implicit operator byte[](BOOT_TRACKING_REPORT command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERAL_DIAGNOSTICS.ID);
                ret.Add(ID);
                if (command.unspecifiedBootCount != null)
                {
                    foreach (var tmp in command.unspecifiedBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.poweronBootCount != null)
                {
                    foreach (var tmp in command.poweronBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.brownoutBootCount != null)
                {
                    foreach (var tmp in command.brownoutBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.watchdogBootCount != null)
                {
                    foreach (var tmp in command.watchdogBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.hardwareresetBootCount != null)
                {
                    foreach (var tmp in command.hardwareresetBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.softwareupdatesuccessBootCount != null)
                {
                    foreach (var tmp in command.softwareupdatesuccessBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.softwareupdatefailedBootCount != null)
                {
                    foreach (var tmp in command.softwareupdatefailedBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.networkexcludedBootCount != null)
                {
                    foreach (var tmp in command.networkexcludedBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.regionconfigurationBootCount != null)
                {
                    foreach (var tmp in command.regionconfigurationBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.nvmcorruptionBootCount != null)
                {
                    foreach (var tmp in command.nvmcorruptionBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.softwareassertBootCount != null)
                {
                    foreach (var tmp in command.softwareassertBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.factoryresetBootCount != null)
                {
                    foreach (var tmp in command.factoryresetBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                if (command.stackoverflowBootCount != null)
                {
                    foreach (var tmp in command.stackoverflowBootCount)
                    {
                        ret.Add(tmp);
                    }
                }
                return ret.ToArray();
            }
        }
        public partial class BOOT_TRACKING_RESET
        {
            public const byte ID = 0x06;
            public static implicit operator BOOT_TRACKING_RESET(byte[] data)
            {
                BOOT_TRACKING_RESET ret = new BOOT_TRACKING_RESET();
                return ret;
            }
            public static implicit operator byte[](BOOT_TRACKING_RESET command)
            {
                List<byte> ret = new List<byte>();
                ret.Add(COMMAND_CLASS_GENERAL_DIAGNOSTICS.ID);
                ret.Add(ID);
                return ret.ToArray();
            }
        }
    }
}

