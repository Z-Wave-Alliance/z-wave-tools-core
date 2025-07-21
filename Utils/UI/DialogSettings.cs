/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace Utils.UI
{
    public class DialogSettings : EntityBase
    {
        public event EventHandler IsFloatingChanged;

        private bool _isFloating;
        public bool IsFloating
        {
            get { return _isFloating; }
            set
            {
                if (_isFloating != value)
                {
                    _isFloating = value;
                    Notify("IsFloating");
                    IsFloatingChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }
        public string UiTheme { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string DialogName { get; set; }
        public bool IsResizable { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsModal { get; set; }
        public bool IsPopup { get; set; }
        public bool IsRecreateOnShow { get; set; }
        public bool CenterOwner { get; set; }
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}