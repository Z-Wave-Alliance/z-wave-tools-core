/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.IO;
using System.Threading.Tasks;

namespace Utils
{
    public static class FileAccessMonitor
    {
        private const int MAX_RETRIES = 5;

        public static void Read(string filePath, Action readAction)
        {
            int attempt = 0;
            while (attempt++ <= MAX_RETRIES)
            {
                try
                {
                    if (File.Exists(filePath))
                    {
                        File.Open(filePath, FileMode.Open, FileAccess.Read).Dispose();
                        readAction.Invoke();
                    }
                    break;
                }
                catch
                {
                    Task.Delay(1000).Wait();
                    continue;
                }
            }
        }
    }
}
