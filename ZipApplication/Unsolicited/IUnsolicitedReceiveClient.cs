/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;

namespace ZWave.ZipApplication
{
    public interface IUnsolicitedReceiveClient : IDisposable
    {
        Action<byte[]> ReceiveData { get; set; }
    }
}
