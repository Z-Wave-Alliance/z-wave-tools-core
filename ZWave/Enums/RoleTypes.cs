/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System.Collections.Generic;

namespace ZWave.Enums
{
    public enum RoleTypes : byte
    {
        None = 0xFF,
        CONTROLLER_CENTRAL_STATIC = 0x00,
        CONTROLLER_SUB_STATIC = 0x01,
        CONTROLLER_PORTABLE = 0x02,
        CONTROLLER_PORTABLE_REPORTING = 0x03,
        END_NODE_PORTABLE = 0x04,
        END_NODE_ALWAYS_ON = 0x05,
        END_NODE_SLEEPING_REPORTING = 0x06,
        END_NODE_SLEEPING_LISTENING = 0x07,
        END_NODE_NETWORK_AWARE = 0x08,
        END_NODE_WAKE_ON_EVENT = 0x09
    }

    public static class RoleTypes_Extensions
    {
        public static readonly List<RoleTypeInfo> RoleTypesInfo = new List<RoleTypeInfo>
        {
            new RoleTypeInfo { RoleType = RoleTypes.None, Name = "Unknown", Abbreviation = "n/a" },
            new RoleTypeInfo { RoleType = RoleTypes.CONTROLLER_CENTRAL_STATIC, Name = "Central Static Controller", Abbreviation = "CSC" },
            new RoleTypeInfo { RoleType = RoleTypes.CONTROLLER_SUB_STATIC, Name = "Sub Static Controller", Abbreviation = "SSC" },
            new RoleTypeInfo { RoleType = RoleTypes.CONTROLLER_PORTABLE, Name = "Portable Controller", Abbreviation = "PC" },
            new RoleTypeInfo { RoleType = RoleTypes.CONTROLLER_PORTABLE_REPORTING, Name = "Portable Reporting Controller", Abbreviation = "RPC" },
            new RoleTypeInfo { RoleType = RoleTypes.END_NODE_PORTABLE, Name = "Portable End Node", Abbreviation = "PEN" },
            new RoleTypeInfo { RoleType = RoleTypes.END_NODE_ALWAYS_ON, Name = "Always On End Node", Abbreviation = "AOEN" },
            new RoleTypeInfo { RoleType = RoleTypes.END_NODE_SLEEPING_REPORTING, Name = "Sleeping Reporting End Node", Abbreviation = "RSEN" },
            new RoleTypeInfo { RoleType = RoleTypes.END_NODE_SLEEPING_LISTENING, Name = "Sleeping Listening End Node", Abbreviation = "LSEN" },
            new RoleTypeInfo { RoleType = RoleTypes.END_NODE_NETWORK_AWARE, Name = "Network Aware End Node", Abbreviation = "NAEN" },
            new RoleTypeInfo { RoleType = RoleTypes.END_NODE_WAKE_ON_EVENT, Name = "Wake On Event End Node", Abbreviation = "WOEEN" }
        };

        public static string GetName(this RoleTypes roleType)
        {
            var info = RoleTypesInfo.Find(r => r.RoleType == roleType);
            return info != null ? info.Name : "Unknown";
        }

        public static string GetAbbreviation(this RoleTypes roleType)
        {
            var info = RoleTypesInfo.Find(r => r.RoleType == roleType);
            return info != null ? info.Abbreviation : "n/a";
        }

        /// <summary>
        /// Determines if the Role Type is a controller.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns>'true' if CSC, SSC, PC or RPC Role Type
        /// </returns>
        public static bool IsController(this RoleTypes roleType)
        {
            switch (roleType)
            {
                case RoleTypes.CONTROLLER_CENTRAL_STATIC:
                case RoleTypes.CONTROLLER_SUB_STATIC:
                case RoleTypes.CONTROLLER_PORTABLE:
                case RoleTypes.CONTROLLER_PORTABLE_REPORTING:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the Role Type is a static controller.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns>'true' if CSC or SSC Role Type</returns>
        public static bool IsStaticController(this RoleTypes roleType)
        {
            switch (roleType)
            {
                case RoleTypes.CONTROLLER_CENTRAL_STATIC:
                case RoleTypes.CONTROLLER_SUB_STATIC:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines if the Role Type is a Central Static Controller.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns>'true' if CSC Role Type </returns>
        public static bool IsCsc(this RoleTypes roleType)
        {
            return roleType == RoleTypes.CONTROLLER_CENTRAL_STATIC;
        }

        /// <summary>
        /// Determines if the Role Type is an end node.
        /// </summary>
        /// <param name="roleType"></param>
        /// <returns>'true' if PEN, AOEN, RSEN, LSEN, NAEN or WOEEN Role Type
        /// </returns>
        public static bool IsEndNode(this RoleTypes roleType)
        {
            switch (roleType)
            {
                case RoleTypes.END_NODE_PORTABLE:
                case RoleTypes.END_NODE_ALWAYS_ON:
                case RoleTypes.END_NODE_SLEEPING_REPORTING:
                case RoleTypes.END_NODE_SLEEPING_LISTENING:
                case RoleTypes.END_NODE_NETWORK_AWARE:
                case RoleTypes.END_NODE_WAKE_ON_EVENT:
                    return true;
                default:
                    return false;
            }
        }

        public class RoleTypeInfo
        {
            public RoleTypes RoleType { get; set; }
            public string Name { get; set; }
            public string Abbreviation { get; set; }
        }
    }
}
