using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.ConstantAndEnum
{
    public enum PrintMode : int
    {
        ManualMode = 0,
        AutoMode = 1
    }
    public enum PLC_ADDRESS
    {
        PLC_SCANOK = 104,
        PLC_TESTRESULT = 105,
        PLC_AUTO_MANUAL_MODE = 106,
        PLC_BYPASS = 107
    }
}
