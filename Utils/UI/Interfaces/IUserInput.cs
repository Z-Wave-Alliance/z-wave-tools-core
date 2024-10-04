/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿namespace Utils.UI.Interfaces
{
    public interface IUserInput
    {
        string AdditionalText { get; set; }
        bool IsAdditionalTextVisible { get; set; }
        string InputData { get; set; }
        bool IsInputDataVisible { get; set; }
        object InputOptions { get; set; }
        bool IsInputOptionsVisible { get; set; }
        bool IsCancelButtonVisible { get; set; }
        object SelectedInputOption { get; set; }
        int SelectedInputOptionIndex { get; set; }
    }
}