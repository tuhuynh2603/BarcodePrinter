using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Models
{
    public class TestResult
    {
        public string DateTime { get; set; }
        public string Mode { get; set; } // Auto/Manual
        public string RS_HM { get; set; }
        public string RS_VM { get; set; } // Pass/Fail
        public string LS_HM { get; set; }
        public string LS_VM { get; set; }
        public string Overall { get; set; } // Pass/Fail
        public string ErrorCode { get; set; } // null nếu Pass
        public string SerialNumber { get; set; } // chỉ Pass
        public string QRCode { get; set; } // chỉ Pass
    }
}
