/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using Utils.UI.Bind;

namespace Utils.UI
{
    public class UList<T> : ObservableCollection<T>, ISubscribeCollection<T> where T : class
    {
        public UList()
        {
            IsSubscribed = true;
        }

        public UList(IEnumerable<T> innerData)
            : base(innerData)
        {
            IsSubscribed = true;
        }

        public bool IsSubscribed { get; private set; }

        public void UnSubscribe()
        {
            IsSubscribed = false;
        }

        public void Subscribe()
        {
            IsSubscribed = true;
        }
    }
}
