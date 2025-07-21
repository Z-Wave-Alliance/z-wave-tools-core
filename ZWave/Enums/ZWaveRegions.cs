/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZWave.Enums
{
    [Serializable]
    /// <summary>
    /// Enumeration Z-Wave regions. Differs from RADIO_CONFIG values coming from PTI 
    /// </summary>
    public enum ZWaveRegions
    {
        REGION_EU = 0,                                          // Radio is located in Region EU. 2 Channel region.
        REGION_US,                                              // Radio is located in Region US. 2 Channel region.
        REGION_ANZ,                                             // Radio is located in Region Australia/New Zealand. 2 Channel region.
        REGION_HK,                                              // Radio is located in Region Hong Kong. 2 Channel region.
        REGION_IN = 5,                                          // Radio is located in Region India. 2 Channel region.
        REGION_IL,                                              // Radio is located in Region Israel. 2 Channel region.
        REGION_RU,                                              // Radio is located in Region Russia. 2 Channel region.
        REGION_CN,                                              // Radio is located in Region China. 2 Channel region.
        REGION_US_LR,                                           // Radio is located in Region US. 2 Channel LR region.
        REGION_EU_LR = 11,                                      // Radio is located in Region EU. 2 Channel LR region.
        REGION_JP = 32,                                         // Radio is located in Region Japan. 3 Channel region.
        REGION_KR,                                              // Radio is located in Region Korea. 3 Channel region.
        REGION_UNDEFINED = 0xFE,
        REGION_DEFAULT = 0xFF,                                  // Radio is located in Library Default Region EU. 2 Channel region.
    }
}
