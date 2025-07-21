/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections;
using Utils.Events;
using ZWave.Exceptions;

namespace ZWave.Layers.Session
{
    public class SessionLayer : ISessionLayer
    {
        public bool SuppressDebugOutput { get; set; }
        public event EventHandler<EventArgs<ActionBase>> ActionChanged;
        public string LogEntryPointClass { get; set; }
        public string LogPrefix { get; set; }
        public ISessionClient CreateClient(ushort sessionId)
        {
            SessionClient sessionClient = new SessionClient(ActionChangeCallback)
            {
                SessionId = sessionId,
                LogEntryPointClass = LogEntryPointClass,
                SuppressDebugOutput = SuppressDebugOutput
            };
            sessionClient.ReleaseSessionIdCallback = (x) =>
            {
                ReleaseSessionId(x);
            };
            sessionClient.RunComponentsDefault();
            return sessionClient;
        }

        protected void ActionChangeCallback(ActionBase actionToken)
        {
            ActionChanged?.Invoke(this, new EventArgs<ActionBase>(actionToken));
        }

        public const int MAX_SESSIONS = 0x7FFF;
        public const int START_SESSION = 0x00;
        private BitArray SessionIds = new BitArray(MAX_SESSIONS + 1);
        private object sessionIdsLocker = new object();
        private ushort lastSessionId = START_SESSION;
        public ushort NextSessionId()
        {
            lock (sessionIdsLocker)
            {
                bool found = false;
                for (int i = 1; i < SessionIds.Length; i++)
                {
                    lastSessionId++;
                    if (lastSessionId <= 0 || lastSessionId >= SessionIds.Length)
                    {
                        lastSessionId = 1;
                    }
                    if (SessionIds[lastSessionId] == false)
                    {
                        SessionIds[lastSessionId] = true;
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    throw new ZWaveException("Session Layer fails to register new Session ID");
                }
                return lastSessionId;
            }
        }

        public void ReleaseSessionId(ushort sessionId)
        {
            lock (sessionIdsLocker)
            {
                SessionIds[sessionId] = false;
            }
        }

        public void ResetSessionIdCounter()
        {
            lock (sessionIdsLocker)
            {
                SessionIds = new BitArray(MAX_SESSIONS + 1);
                lastSessionId = START_SESSION;
            }
        }
    }
}
