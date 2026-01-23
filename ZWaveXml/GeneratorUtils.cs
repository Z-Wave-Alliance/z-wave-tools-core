/// SPDX-License-Identifier: BSD-3-Clause
/// SPDX-FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ZWave.Xml
{
    public static class GeneratorUtils
    {
        public const string NEW_LINE = "\n"; // Unix line ending, no matter the platform

        private const string SPDX_LICENSE_ID_BSD_3 = "SPDX-" + "License-Identifier: BSD-3-Clause";
        private const string SPDX_FILE_COPYRIGHT_ZWA = "SPDX-" +"FileCopyrightText: Z-Wave-Alliance https://z-wavealliance.org";

        /// <summary>
        /// Adds license info as C#-style documentation comments
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddLicenseInfo(this StreamWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException();
            }
            writer.WriteLine($"/// {SPDX_LICENSE_ID_BSD_3}");
            writer.WriteLine($"/// {SPDX_FILE_COPYRIGHT_ZWA}");
        }

        /// <summary>
        /// Adds license info as XML comments
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddLicenseInfo(this XmlWriter writer)
        {
            if (writer == null)
            {
                throw new ArgumentNullException();
            }
            writer.WriteComment(
                $"{NEW_LINE}" +
                $"{SPDX_LICENSE_ID_BSD_3}{NEW_LINE}" +
                $"{SPDX_FILE_COPYRIGHT_ZWA}{NEW_LINE}");
        }
    }
}
