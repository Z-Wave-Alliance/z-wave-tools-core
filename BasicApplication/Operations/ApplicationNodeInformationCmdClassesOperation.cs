/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.BasicApplication.Enums;

namespace ZWave.BasicApplication.Operations
{
    public class ApplicationNodeInformationCmdClassesOperation : ControlNApiOperation
    {
        private byte[] NodeParameters { get; set; }
        private byte[] UnsecureNodeParameters { get; set; }
        private byte[] SecureNodeParameters { get; set; }
        public ApplicationNodeInformationCmdClassesOperation(byte[] nodeParameters, byte[] unsecureNodeParameters, byte[] secureNodeParameters)
            : base(CommandTypes.CmdSerialApiApplNodeInformationCmdClasses)
        {
            NodeParameters = nodeParameters;
            UnsecureNodeParameters = unsecureNodeParameters;
            SecureNodeParameters = secureNodeParameters;
        }

        protected override byte[] CreateInputParameters()
        {
            List<byte> ret = new List<byte>();
            if (NodeParameters != null)
            {
                ret.Add((byte)NodeParameters.Length);
                ret.AddRange(NodeParameters);
            }
            else
                ret.Add(0x00);
            if (UnsecureNodeParameters != null)
            {
                ret.Add((byte)UnsecureNodeParameters.Length);
                ret.AddRange(UnsecureNodeParameters);
            }
            else
                ret.Add(0x00);
            if (SecureNodeParameters != null)
            {
                ret.Add((byte)SecureNodeParameters.Length);
                ret.AddRange(SecureNodeParameters);
            }
            else
                ret.Add(0x00);

            return ret.ToArray();
        }
    }
}
