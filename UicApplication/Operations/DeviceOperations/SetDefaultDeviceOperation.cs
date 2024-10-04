/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using UicApplication;
using UicApplication.Data;
using UicApplication.Enums;
using UicApplication.Clusters;
using ZWave.TextApplication.Operations;

namespace ZWave.UicApplication
{
    public class SetDefaultDeviceOperation : SendOperation
    {
        public string _unId;

        public SetDefaultDeviceOperation()
            : base("network leave", System.Text.Encoding.ASCII)
        {
        }

        public override string AboutMe()
        {
            return string.Format("Reseting Uic End Device = {0}", _unId);
        }      

        public new SetDefaultResult SpecificResult
        {
            get { return (SetDefaultResult)Result; }
        }

        protected override ActionResult CreateOperationResult()
        {
            return new SetDefaultResult();
        }
    }
}
