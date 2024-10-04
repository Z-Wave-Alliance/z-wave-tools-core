/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using System.ComponentModel;

namespace ZWave.Xml.FrameHeader
{
    [Serializable]
    public class CommunicationPort : INotifyPropertyChanged
    {
        public CommunicationPort()
        {

        }
        public CommunicationPort(string sourceName)
        {
            Name = sourceName;
            Order = sourceName;
            UpdateTextFromName();
        }

        public void UpdateTextFromName()
        {
            Text = Name;
            if (Name.StartsWith("COM", StringComparison.CurrentCultureIgnoreCase)
                && Name.Length > 3
                && int.TryParse(Name.Substring(3), out int number)
                )
            {
                Text = "COM " + number;
                Order = number.ToString("0000");
            }
        }


        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string _order;
        public string Order
        {
            get
            {
                return _order;
            }
            set
            {
                _order = value;
                RaisePropertyChanged("Order");
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }

        private string _library;
        public string Library
        {
            get
            {
                return _library;
            }
            set
            {
                _library = value;
                RaisePropertyChanged("Library");
            }
        }

        private string _version;
        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                _version = value;
                RaisePropertyChanged("Version");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
