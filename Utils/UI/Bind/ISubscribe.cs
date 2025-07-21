/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace Utils.UI.Bind
{
    public interface ISubscribe
    {
        void UnSubscribe();
        void Subscribe();
        bool IsSubscribed { get; }
    }
}
