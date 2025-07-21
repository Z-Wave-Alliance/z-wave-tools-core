/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Text;
using Utils;

namespace ZWave.TextApplication.Operations
{
    public class SendOperation : ActionBase
    {
        private byte[] TextData { get; set; }
        private string Text { get; set; }
        public SendOperation(byte[] textData)
            : base(true)
        {
            TextData = textData;
        }

        public SendOperation(string text, Encoding textEncoding)
            : base(true)
        {
            Text = text;
            TextData = textEncoding.GetBytes(text);
        }


        private CommandMessage message;
        protected override void CreateWorkflow()
        {
            ActionUnits.Add(new StartActionUnit(SetStateCompleting, 0, message));
        }

        protected override void CreateInstance()
        {
            message = new TextApiMessage(TextData);
            message.IsNoAck = true;
        }

        public override string AboutMe()
        {
            if (Text != null)
            {
                string tt = Text.Replace("\r", "\\r");
                return string.Format("Tx:\"{0}\"", tt);
            }
            return Tools.GetHex(TextData);
        }
    }
}
