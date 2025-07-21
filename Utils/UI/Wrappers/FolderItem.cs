/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections;

namespace Utils.UI.Wrappers
{
    public class FolderItem
    {
        public string Name { get; set; }
        public IEnumerable Items { get; set; }
        public object Parent { get; set; }
    }
}
