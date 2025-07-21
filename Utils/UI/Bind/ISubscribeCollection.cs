/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Utils.UI.Bind
{
    public interface ISubscribeCollection<T> : ISubscribe, IList<T> where T : class
    {
        event NotifyCollectionChangedEventHandler CollectionChanged;
    }
}
