/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using ZWave.Layers.Frame;
using System.Collections.Generic;
using ZWave.Enums;
using Utils.Threading;

namespace ZWave.Layers
{
    public interface ISessionClient : IDisposable
    {
        bool SuppressDebugOutput { get; set; }
        ushort SessionId { get; set; }
        bool IsHandleFrameEnabled { get; set; }
        void SetFuncId(byte value);
        Func<ActionHandlerResult, bool> SendFramesCallback { get; set; }
        Func<ActionBase, ActionBase> PostSubstituteAction { get; set; }
        Action<ActionBase> ActionStartedCallback { get; set; }
        Action<ActionBase> ActionCompletedCallback { get; set; }
        void HandleActionCase(IActionCase actionCase);
        ActionToken ExecuteAsync(IActionItem action);
        void RunComponents(ITimeoutManager timeoutManager, IConsumerThread<IActionCase> handleCases);
        void RunComponentsDefault();
        void Cancel(Type type);
        void Cancel(ActionToken token);
        void ResetSubstituteManagers();
        ISubstituteManager GetSubstituteManager(Type type);
        void AddSubstituteManager(ISubstituteManager sm, params ActionBase[] actions);
        void ClearSubstituteManagers();
    }
}
