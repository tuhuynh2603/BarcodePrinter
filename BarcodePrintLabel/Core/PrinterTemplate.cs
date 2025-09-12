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
            return $"^XA^FO50,50^ADN,36,20^FD{serialNumber}^FS^XZ";
        }
    }
}
