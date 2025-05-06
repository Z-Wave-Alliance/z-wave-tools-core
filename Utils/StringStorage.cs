/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Text;

namespace Utils
{
    public class StringStorage
    {
        public StringBuilder ActiveStringBuilder { get; private set; }
        public StringBuilder StoreStringBuilder { get; private set; }
        private readonly object _locker = new object();
        public StringStorage()
        {
            ActiveStringBuilder = new StringBuilder();
            StoreStringBuilder = new StringBuilder();
        }

        public string Get()
        {
            lock (_locker)
            {
                ActiveStringBuilder = StoreStringBuilder;
                StoreStringBuilder = new StringBuilder();
            }
            if (ActiveStringBuilder.Length > 0)
            {
                return ActiveStringBuilder.ToString();
            }
            return string.Empty;
        }

        public void Add(string str)
        {
            lock (_locker)
            {
                StoreStringBuilder.AppendLine(str);
            }
        }
    }
}
