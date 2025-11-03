/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
/// SPDX-FileCopyrightText: Z-Wave Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;

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
        private static readonly IReadOnlyCollection<RoleTypeInfo> _roleTypesInfo = new List<RoleTypeInfo>
        {
            new RoleTypeInfo(RoleTypes.None, "Unknown", "n/a"),
            new RoleTypeInfo(RoleTypes.CONTROLLER_CENTRAL_STATIC, "Central Static Controller", "CSC"),
            new RoleTypeInfo(RoleTypes.CONTROLLER_SUB_STATIC, "Sub Static Controller", "SSC"),
            new RoleTypeInfo(RoleTypes.CONTROLLER_PORTABLE, "Portable Controller", "PC"),
            new RoleTypeInfo(RoleTypes.CONTROLLER_PORTABLE_REPORTING, "Portable Reporting Controller", "RPC"),
            new RoleTypeInfo(RoleTypes.END_NODE_PORTABLE, "Portable End Node", "PEN"),
            new RoleTypeInfo(RoleTypes.END_NODE_ALWAYS_ON, "Always On End Node", "AOEN"),
            new RoleTypeInfo(RoleTypes.END_NODE_SLEEPING_REPORTING, "Sleeping Reporting End Node", "RSEN"),
            new RoleTypeInfo(RoleTypes.END_NODE_SLEEPING_LISTENING, "Sleeping Listening End Node", "LSEN"),
            new RoleTypeInfo(RoleTypes.END_NODE_NETWORK_AWARE, "Network Aware End Node", "NAEN"),
            new RoleTypeInfo(RoleTypes.END_NODE_WAKE_ON_EVENT, "Wake On Event End Node", "WOEEN")
        };

        public static string GetName(this RoleTypes roleType)
        {
            RoleTypeInfo info = _roleTypesInfo.FirstOrDefault(i => i.RoleType == roleType);
            return !info.Name.IsNullOrEmpty() ? info.Name : "Unknown";
        }

        public static string GetAbbreviation(this RoleTypes roleType)
        {
            RoleTypeInfo info = _roleTypesInfo.FirstOrDefault(i => i.RoleType == roleType);
            return !info.Abbreviation.IsNullOrEmpty() ? info.Abbreviation : "n/a";
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

        public readonly struct RoleTypeInfo
        {
            public RoleTypeInfo(RoleTypes roleType, string name, string abbreviation)
            {
                RoleType = roleType;
                Name = name;
                Abbreviation = abbreviation;
            }

            public readonly RoleTypes RoleType;
            public readonly string Name;
            public readonly string Abbreviation;
        }
    }
}
