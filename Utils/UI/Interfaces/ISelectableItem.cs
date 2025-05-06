/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils.UI.Interfaces
{
    public interface ISelectableItem<T>
    {
        bool IsSelected { get; set; }
        bool IsEnabled { get; set; }
        T Item { get; set; }
        string ToString();
        void RefreshBinding();
    }
}
