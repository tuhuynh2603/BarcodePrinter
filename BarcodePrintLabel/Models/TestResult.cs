using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Models
{
    public class TestResult
    {
        public int Id { get; set; }  // Primary Key

        public string SerialNumber { get; set; }
        public string QRCode { get; set; }
        public string LS_HORIZONTAL             { get; set; }
        public string LS_VERTICAL           { get; set; } 
        public string LS_HORIZONTAL2            { get; set; }
        public string LS_VERTICAL2          { get; set; }
        public string RS_HORIZONTAL             { get; set; }
        public string RS_VERTICAL           { get; set; } 
        public string RS_HORIZONTAL2            { get; set; }
        public string RS_VERTICAL2          { get; set; }
        public string RESULT                    { get; set; } 
        public string ERROR_CODE            { get; set; } 
        public DateTime DateTime              { get; set; }
        public string CAVITY { get; set; }
    }


}
