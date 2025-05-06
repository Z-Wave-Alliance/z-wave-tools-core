/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UicApplication;
using UicApplication.Enums;
using ZWave;

namespace ZWave.UicApplication
{
    public class PublishOperation : UicApiOperation
    {
        internal string _unid;
        internal string _cluster;
        internal string _payload;
        internal string _endPoint;
        internal bool _retain = false;
        public PublishOperation(string unid, string cluster, string payload) : base(true)
        {
            _unid = "ucl/by-unid/" + unid;
            _cluster = cluster;
            if (payload == null)
            {
                _payload = "{}";
            }
            else
            {
                _payload = payload;
            }
        }
        public PublishOperation(string unid, string endPoint, string cluster, string payload) : base(true)
        {
            if (endPoint == null)
            {
                _endPoint = "ep0";
            }
            else
            {
                _endPoint = endPoint;
            }
            _unid = "ucl/by-unid/" + unid + "/" + _endPoint;
            _cluster = cluster;
            if (payload == null)
            {
                _payload = "{}";
            }
            else
            {
                _payload = payload;
            }
        }
        public PublishOperation(string cluster, string payload) : base(true)
        {
            _cluster = cluster;
            if (payload == null)
            {
                _payload = "{}";
            }
            else
            {
                _payload = payload;
            }
        }
        public PublishOperation(string cluster, string payload, bool retain) : base(true)
        {
            _cluster = cluster;
            _retain = retain;
            if (payload == null && !_retain)
            {
                _payload = "{}";
            }
            else if (payload == null && !_retain)
            {
                _payload = "";
            }
            else
            {
                _payload = payload;
            }            
        }
        public PublishOperation(string unid, string cluster, string payload, int groupid) : base(true)
        {
            _unid = "ucl/by-group/" + groupid.ToString();
            _cluster = cluster;
            if (payload == null)
            {
                _payload = "{}"; 
            }
            else
            {
                _payload = payload;
            }    
        }

        private UicApiMessage message;

        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, AckTimeout, message));
        }

        protected override void CreateInstance()
        {
            message = new UicApiMessage(_unid, _cluster, _payload, true, _retain);
        }

    }
}
