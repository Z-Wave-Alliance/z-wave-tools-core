/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using ZWave.BasicApplication.Enums;
using ZWave.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class ApplicationNodeInformationOperation : ControlNApiOperation
    {
        private byte Generic { get; set; }
        private byte Specific { get; set; }
        private byte[] NodeParameters { get; set; }
        private DeviceOptions DeviceOptions { get; set; }

        public ApplicationNodeInformationOperation(DeviceOptions deviceOptions, byte generic, byte specific, byte[] nodeParameters)
            : base(CommandTypes.CmdSerialApiApplNodeInformation)
        {
            DeviceOptions = deviceOptions;
            Generic = generic;
            Specific = specific;
            NodeParameters = nodeParameters;
        }

        protected override byte[] CreateInputParameters()
        {
            byte[] ret = new byte[NodeParameters.Length + 4];
            ret[0] = (byte)DeviceOptions;
            ret[1] = Generic;
            ret[2] = Specific;
            ret[3] = (byte)NodeParameters.Length;
            for (int i = 0; i < NodeParameters.Length; i++)
            {
                ret[i + 4] = NodeParameters[i];
            }
            return ret;
        }
    }
}
