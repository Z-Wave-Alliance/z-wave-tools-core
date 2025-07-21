/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils.UI.Interfaces
{
    public interface IDialog
    {       
        Action CloseCallback { get; set; }
        bool IsOk { get; set; }
        bool ShowDialog();
        void Close();        
    }
}