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
        public string DateTime              { get; set; }


        public static TestResult CreateHeaderResult()
        {
            return new TestResult()
            {
                SerialNumber = "Serial Number",
                QRCode = "QR Code",
                LS_HORIZONTAL = "LS_HORIZONTAL",
                LS_VERTICAL = "LS_VERTICAL",
                LS_HORIZONTAL2 = "LS_HORIZONTAL2",
                LS_VERTICAL2 = "LS_VERTICAL2",
                RS_HORIZONTAL = "RS_HORIZONTAL",
                RS_VERTICAL = "RS_VERTICAL",
                RS_HORIZONTAL2 = "RS_HORIZONTAL2",
                RS_VERTICAL2 = "RS_VERTICAL2",
                RESULT = "RESULT",
                ERROR_CODE = "ERROR_CODE",
                DateTime = "Date Time",
            };
        }
    }


}
