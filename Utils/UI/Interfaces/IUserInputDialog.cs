/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils.UI.Interfaces
{
    public interface IUserInputDialog : IDialog
    {
        IUserInput State { get; set; }
        bool HasCancel { get; set; }
    }
}
