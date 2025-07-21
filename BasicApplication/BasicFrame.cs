/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.BasicApplication
{
    public class BasicFrame
    {
        public BasicFrame(DataFrame dataFrame)
        {
            Command = dataFrame.ToString();
        }

        public string Command { get; set; }
    }
}
