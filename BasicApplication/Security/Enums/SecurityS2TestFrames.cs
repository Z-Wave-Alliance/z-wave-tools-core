/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com

namespace ZWave.BasicApplication.Security
{
    public enum SecurityS2TestFrames
    {
        InjectCommand,
        KEXGet,
        KEXReport,
        KEXSet,
        PublicKeyReportB,
        PublicKeyReportA,
        KEXSetEcho,
        KEXReportEcho,
        NetworkKeyGet_S0,
        NetworkKeyReport_S0,
        NetworkKeyVerify_S0,
        NetworkKeyGet_S2Unauthenticated,
        NetworkKeyReport_S2Unauthenticated,
        NetworkKeyVerify_S2Unauthenticated,
        NetworkKeyGet_S2Authenticated,
        NetworkKeyReport_S2Authenticated,
        NetworkKeyVerify_S2Authenticated,
        NetworkKeyGet_S2Access,
        NetworkKeyReport_S2Access,
        NetworkKeyVerify_S2Access,
        TransferEndA_S0,
        TransferEndA_S2Unauthenticated,
        TransferEndA_S2Authenticated,
        TransferEndA_S2Access,
        TransferEndB,
        NonceGet,
        NonceReport,
        MessageEncapsulation,
        CommandsSupportedReport,
        InclusionInititate1,
        InclusionInititate2,
        InclusionComplete1,
        InclusionComplete2
    }
}
