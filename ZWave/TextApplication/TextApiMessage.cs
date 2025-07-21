/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace ZWave.TextApplication
{
    public class TextApiMessage : CommandMessage
    {
        public TextApiMessage(params byte[] inputParameters)
        {
            AddData(inputParameters);
        }
    }
}
