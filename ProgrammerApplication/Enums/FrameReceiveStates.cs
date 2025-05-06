using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZWave.ProgrammerApplication.Enums
{
    public enum FrameReceiveStates
    {
        FRS_SOF_HUNT = 0,
        FRS_DATA = 1,
        FRS_EOF = 2,
    }
}
