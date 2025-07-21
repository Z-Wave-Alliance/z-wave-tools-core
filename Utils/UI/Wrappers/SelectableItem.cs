/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using Utils.UI.Interfaces;

namespace Utils.UI.Wrappers
{
    public class SelectableItem<T> : ISelectableItem<T>
    {
        public bool IsSelected { get; set; }
        public bool IsEnabled { get; set; } = true;
        public T Item { get; set; }

        public SelectableItem(T item)
        {
            Item = item;
        }

        public override string ToString()
        {
            var obj = Item as object;
            if (obj != null)
                return obj.ToString();

            return base.ToString();
        }

        public void RefreshBinding() { }
    }

    public class SelectableEntity<T> : EntityBase, ISelectableItem<T>
    {
        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                Notify("IsSelected");
            }
        }

        private bool _isEnabled = true;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                Notify("IsEnabled");
            }
        }

        private T _item;
        public T Item
        {
            get { return _item; }
            set
            {
                _item = value;
                Notify("Item");
            }
        }

        public SelectableEntity(T item)
        {
            Item = item;
        }

        public override string ToString()
        {
            var obj = Item as object;
            if (obj != null)
                return obj.ToString();

            return base.ToString();
        }

        public void RefreshBinding() { }
    }
}
