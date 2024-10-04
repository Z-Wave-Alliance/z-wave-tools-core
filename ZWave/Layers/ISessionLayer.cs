/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
﻿using System;
using Utils.Events;

namespace ZWave.Layers
{
    /// <summary>
    /// Provides the features required to support communication sessions.
    /// </summary>
    public interface ISessionLayer
    {
        bool SuppressDebugOutput { get; set; }
        ISessionClient CreateClient(ushort sessionId);
        event EventHandler<EventArgs<ActionBase>> ActionChanged;
        ushort NextSessionId();
        void ResetSessionIdCounter();
        void ReleaseSessionId(ushort sessionId);

        /// <summary>
        /// Add debug info about line number in the specified class in the stack trace
        /// </summary>
    }
}
