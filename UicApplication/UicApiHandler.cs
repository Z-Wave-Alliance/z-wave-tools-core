/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Text.RegularExpressions;
using UicApplication.Data;
using Utils;
using ZWave;

namespace UicApplication
{
    class UicApiHandler : CommandHandler
    {
        /// <summary>
        /// Right now is a direct copy from the mirror class on ZipApplication library
        /// This will have to be modified taking in to account Dotdot frames
        /// </summary>
        public string Topic { get; set; }
        public string Payload { get; set; }
        private UicApiHandler()
        {
        }

        public UicApiHandler(string topic)
        {
            Topic = topic;
        }

        protected bool IsUicExpectedData(string receivedTopic)
        {
            if (Topic == null)
            {
                return true;
            }
            else
            {
                bool isWildcardUsed = Topic.Contains("+") || Topic.Contains("#");

                if (isWildcardUsed)
                {
                    string pattern = Regex.Escape(Topic).Replace("\\+", "[^\\/]+");
                    return Regex.IsMatch(receivedTopic, pattern);
                }
                else
                {
                    return receivedTopic.Contains(Topic);
                }

            }
        }

        public override bool WaitingFor(IActionCase actionCase)
        {
            bool ret = false;
            if (actionCase is DataFrame receivedFrame)
            {
                ret = IsUicExpectedData(receivedFrame.MqttTopic);
                "{0}: {1} - {2}"._DLOG(ret, actionCase.ToString(), receivedFrame.MqttPayload);
                if (ret)
                {
                    DataFrame = receivedFrame;
                }
            }
            return ret;
        }
    }
}
