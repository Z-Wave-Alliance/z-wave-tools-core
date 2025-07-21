/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using ZWave.Layers.Frame;
using ZWave.Enums;

namespace ZWave.Layers
{
    public interface ISubstituteManager
    {
        SubstituteIncomingFlags Id { get; }
        CustomDataFrame SubstituteIncoming(CustomDataFrame packet, out ActionBase additionalAction /*Optional*/, out ActionBase completeAction /*Optional*/);
        bool OnIncomingSubstituted(CustomDataFrame dataFrameOri, CustomDataFrame dataFrameSub, List<ActionHandlerResult> ahResults, out ActionBase additionalAction);
        ActionBase SubstituteAction(ActionBase runningOperation);
        List<ActionToken> GetRunningActionTokens();
        void AddRunningActionToken(ActionToken token);
        void RemoveRunningActionToken(ActionToken token);
        void SetDefault();
        void Suspend();
        void Resume();
        bool IsActive { get; }
    }
}
