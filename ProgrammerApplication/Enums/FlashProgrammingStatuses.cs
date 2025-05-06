using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum FlashProgrammingStatuses
    {
        /// <summary>
        /// Done.
        /// </summary>
        DONE = '\r',
        /// <summary>
        /// Success.
        /// </summary>
        SUCCESS = '1',
        /// <summary>
        /// Fail.
        /// </summary>
        FAIL = '0'
    }
}
