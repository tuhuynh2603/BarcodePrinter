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
    public enum PLC_ADDRESS : int
    {
        //PLC_BYPASS = 100,
        APP_EVENT = 110,
        APP_SCAN_RESULT = 1
        //PLC_AUTO_MANUAL_MODE = 106,
         
    }
}
