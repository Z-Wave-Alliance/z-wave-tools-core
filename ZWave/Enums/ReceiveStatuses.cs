/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;

namespace ZWave.Enums
{
    /* Received frame status flags */
    /**
     *  \defgroup RECEIVE_STATUS Status codes for receiving frames.
     * \addtogroup RECEIVE_STATUS
     * @{
     */
    [Flags]
    public enum ReceiveStatuses
    {
        /**
         * A response route is locked by the application
         */
        RoutedBusy = 0x01,
        /**
         * Received at low output power level, this must
         * have the same value as TRANSMIT_OPTION_LOW_POWER
         */
        LowPower = 0x02,
        /**
         * Received frame is broadcast frame  (rxOptions == xxxx01xx)
         */
        TypeBroad = 0x04,
        /**
         * Received frame is multicast frame (rxOptions == xxxx10xx)
         */
        TypeMulti = 0x08,
        /**
         * Received frame is an explore frame (rxOptions == xxx1xxxx)
         * Only TYPE_BROAD can be active at the same time as TYPE_EXPLORE
         */
        TypeExplore = 0x10,
        /**
         * Received frame is not send to me (rxOptions == x1xxxxxx)
         * - useful only in promiscuous mode
         */
        ForeignFrame = 0x40
    }
}
