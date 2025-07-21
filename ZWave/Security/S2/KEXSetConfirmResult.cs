/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Silicon Laboratories Inc. https://www.silabs.com
using System.Collections.Generic;
using System.Linq;
using ZWave.Enums;

namespace ZWave.Security.S2
{
    public class KEXSetConfirmResult
    {
        public bool IsConfirmed { get; set; }
        public bool IsAllowedCSA { get; set; }
        public bool IsSupportedSecurityS0 { get; private set; }
        public bool IsSupportedSecurityS2_UNAUTHENTICATED { get; private set; }
        public bool IsSupportedSecurityS2_AUTHENTICATED { get; private set; }
        public bool IsSupportedSecurityS2_ACCESS { get; private set; }

        private List<SecuritySchemes> _grantedSchemes;
        public List<SecuritySchemes> GrantedSchemes
        {
            get { return _grantedSchemes; }
            set
            {
                _grantedSchemes = value;
                IsSupportedSecurityS0 = _grantedSchemes.Any(i => i == SecuritySchemes.S0);
                IsSupportedSecurityS2_UNAUTHENTICATED = _grantedSchemes.Any(i => i == SecuritySchemes.S2_UNAUTHENTICATED);
                IsSupportedSecurityS2_AUTHENTICATED = _grantedSchemes.Any(i => i == SecuritySchemes.S2_AUTHENTICATED);
                IsSupportedSecurityS2_ACCESS = _grantedSchemes.Any(i => i == SecuritySchemes.S2_ACCESS);
            }
        }

        public KEXSetConfirmResult(bool isCheckedCSA, bool isCheckedS2_ACCESS, bool isCheckedS2_AUTHENTICATED, bool isCheckedS2_UNAUTHENTICATED, bool isCheckedS0)
        {
            IsConfirmed = true;
            IsAllowedCSA = isCheckedCSA;
            GrantedSchemes = new List<SecuritySchemes>();
            if (isCheckedS2_ACCESS)
                GrantedSchemes.Add(SecuritySchemes.S2_ACCESS);
            if (isCheckedS2_AUTHENTICATED)
                GrantedSchemes.Add(SecuritySchemes.S2_AUTHENTICATED);
            if (isCheckedS2_UNAUTHENTICATED)
                GrantedSchemes.Add(SecuritySchemes.S2_UNAUTHENTICATED);
            if (isCheckedS0)
                GrantedSchemes.Add(SecuritySchemes.S0);
        }

        public KEXSetConfirmResult()
        {
            GrantedSchemes = new List<SecuritySchemes>();
        }

        public static byte ConvertToNetworkKeyFlags(params SecuritySchemes[] schemes)
        {
            byte ret = 0;
            if (schemes != null && schemes.Length > 0)
            {
                foreach (var item in schemes)
                {
                    if (item == SecuritySchemes.S2_UNAUTHENTICATED)
                    {
                        ret = (byte)(ret | (byte)NetworkKeyS2Flags.S2Class0);
                    }
                    else if (item == SecuritySchemes.S2_AUTHENTICATED)
                    {
                        ret = (byte)(ret | (byte)NetworkKeyS2Flags.S2Class1);
                    }
                    else if (item == SecuritySchemes.S2_ACCESS)
                    {
                        ret = (byte)(ret | (byte)NetworkKeyS2Flags.S2Class2);
                    }
                    else if (item == SecuritySchemes.S0)
                    {
                        ret = (byte)(ret | (byte)NetworkKeyS2Flags.S0);
                    }
                }
            }
            return ret;
        }

        public static KEXSetConfirmResult Default { get; } = new KEXSetConfirmResult();
    }
}
