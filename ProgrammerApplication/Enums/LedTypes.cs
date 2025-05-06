using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum LedTypes : int
    {
        /// <summary>
        /// Pass Led.
        /// </summary>
        Pass = 0,
        /// <summary>
        /// Error Led.
        /// </summary>
        Error = 1,
        /// <summary>
        /// Busy Led.
        /// </summary>
        Busy = 2,
    }
}
