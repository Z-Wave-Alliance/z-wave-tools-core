/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using Utils.UI;

namespace Utils
{
    public interface IDataSource
    {
        string SourceName { get; }
        string Alias { get; }
        string Args { get; }
        bool IsActive { get; set; }
        string SourceId { get; }
        string Description { get; set; }
        string Version { get; set; }
        bool Validate();
    }

    public class DataSourceBase : EntityBase, IDataSource
    {
        public string SourceName { get; set; }
        public string Alias { get; set; }
        public string Args { get; set; }
        public bool IsActive { get; set; }

        protected string _sourceId;
        public string SourceId
        {
            get { return _sourceId ?? (_sourceId = GenerateSourceId()); }
        }
        protected virtual string GenerateSourceId()
        {
            return SourceName;
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                Notify("Description");
            }
        }

        private string _version;
        public string Version
        {
            get { return _version; }
            set
            {
                _version = value;
                Notify("Version");
            }
        }

        public virtual bool Validate()
        {
            return true;
        }
    }
}
