/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZWave;

namespace UicApplication.Operations
{
    public class SubscribeOperation : UicApiOperation
    {
        internal string _unid;
        internal string _cluster;

        public SubscribeOperation(string unid, string cluster, string endPoint) : base(true)
        {
            if (unid == "+" && endPoint == "+")
            {
                _unid = "ucl/by-unid/" + unid + "/" + endPoint;
                _cluster = cluster;
            }
            else
            {
                _unid = "ucl/by-unid/" + unid + "/ep/" + endPoint;
                _cluster = cluster;
            }            
        }
        public SubscribeOperation(string unid, string cluster) : base(true)
        {
            if (unid == null)
            {
                unid = "+";
            }
            _unid = "ucl/by-unid/" + unid;
            _cluster = cluster;
        }

        public SubscribeOperation(string topic) : base(true)
        {
            _unid = "";
            _cluster = topic;
        }

        private UicApiMessage message;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, AckTimeout, message));
        }

        protected override void CreateInstance()
        {
            message = new UicApiMessage(_unid, _cluster);
        }
    }
}
