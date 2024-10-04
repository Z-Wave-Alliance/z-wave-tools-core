/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Utils.UI.Bind
{
    public interface ISubscribeCollectionFactory
    {
        Collection<T> CreateCollection<T>() where T : class;
        Collection<T> CreateCollection<T>(IEnumerable<T> innerData) where T : class;
        ISubscribeCollection<T> Create<T>() where T : class;
        ISubscribeCollection<T> Create<T>(IEnumerable<T> innerData) where T : class;
    }
}
