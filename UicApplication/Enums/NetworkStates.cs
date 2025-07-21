/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
namespace UicApplication.Enums
{
    /// <summary>
    /// Network Management cluster static values
    /// </summary>
    public class NetworkStates
    {
        public const string Idle = "idle";
        public const string AddNode = "add node";
        public const string RemoveNode = "remove node";
        public const string RemoveFailed = "RemoveOffline";
        public const string JoinNetwork = "join network";
        public const string LeaveNetwork = "leave network";
        public const string NodeInterview = "node interview";
        public const string NetworkRepair = "network repair";
        public const string NetworkMaintenance = "network maintenance";
        public const string Reset = "reset";
        public const string TemporarilyOffline = "temporarily offline";
        public const string ScanMode = "scan mode";
    }
    public class NetworkStatesValues
    {
        public const string NodeInterview = "INTERVIEWING";
        public const string Included = "INCLUDED";
        public const string OnlineInterviewing = "Online interviewing";
        public const string OnlineFunctional = "Online functional";
        public const string Reset = "reset";
        public const string Removed = "remove node";
        public const string Idle = "idle";
    }
}
