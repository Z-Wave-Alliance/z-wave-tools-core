/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Text;
using Utils;

namespace ZWave.TextApplication.Operations
{
    public class ListenOperation : ActionBase
    {
        private Action<string> ListenCallback { get; set; }
        private string receivedText;
        private Encoding TextEncoding { get; set; }
        public ListenOperation(Encoding textEncoding, Action<string> listenCallback)
            : base(true)
        {
            TextEncoding = textEncoding;
            ListenCallback = listenCallback;
        }

        private CommandHandler handler;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(null, 0));
            ActionUnits.Add(new DataReceivedUnit(handler, OnReceived));
        }

        protected override void CreateInstance()
        {
            handler = new CommandHandler();
            handler.AddConditions(ByteIndex.AnyValue);
        }

        private void OnReceived(DataReceivedUnit ou)
        {
            var receivedData = ou.DataFrame.Payload;
            
            if (TextEncoding != null)
                receivedText += TextEncoding.GetString(receivedData);
            if (receivedText.Contains("\n") || receivedText.Contains("\r"))
            {
                ListenCallback(receivedText);
                receivedText = string.Empty;
            }

        }             
    }
}
