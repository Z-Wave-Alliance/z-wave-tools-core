/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils.UI
{
    public interface IDispatch
    {
        bool CheckAccess();
        void BeginInvoke(Action action);
        void Invoke(Action action);
        bool InvokeBackground(Action action, int timeoutMs);
        bool InvokeBackground(Action action);
    }
}
