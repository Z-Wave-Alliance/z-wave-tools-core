/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils.UI
{
    public class ValueEntity<T> : EntityBase
    {
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                _value = value;
                Notify("Value");
            }
        }
    }
}
