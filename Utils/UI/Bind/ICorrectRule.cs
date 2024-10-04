/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils.UI.Bind
{
    public interface ICorrectRule
    {
        object Correct(string value);
        string ToString(object value);
        bool HasName(string name);

        bool IsValid(object value);
        string ValidationMessage { get; set; }
    }
}
