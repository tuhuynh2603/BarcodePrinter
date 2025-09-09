using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Core
{
    public static class PrinterTemplate
    {
        public static string MakePrinterTemplateCommand(string serialNumber)
        {
            string template = @"
                            INPUT OFF

                            PP 50,50
                            FT ""Swiss 721 BT"",10
                            PT ""Serial: {SN}""

                            PP 50,150
                            BARSET ""CODE128"",2,1,100,0
                            PB ""{SN}""

                            PF
                            ";

            return template.Replace("{SN}", serialNumber);
        }
    }
}
