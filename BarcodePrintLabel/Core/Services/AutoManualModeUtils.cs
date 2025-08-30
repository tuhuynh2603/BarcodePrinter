using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Models;
using BarcodePrintLabel.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Core.Services
{

    public static class AuToManualModeUtils
    {
        public static void StartSequence(PrintMode mode, MainViewModel mainViewModel)
        {
            Console.WriteLine($"Starting test in {mode}...");

            if (mode is PrintMode.AutoMode)
            {
                DoAutoMode(mainViewModel);
            }
            else if (mode is PrintMode.ManualMode)
            {
                DoManualMode(mainViewModel);
            }
            else
            {
                throw new ArgumentException("Invalid operation mode specified.");
            }
        }

        private static void DoAutoMode(MainViewModel mainViewModel)
        {
            // Tạo Thread
            // wait signal
            // Scan
            // Check data valid
            // Send data to UI / Excel
            // giao tiếp printer
            //Print
        }

        private static void DoManualMode(MainViewModel mainViewModel)
        {
            var data = GetDataResult(mainViewModel.printerSerial.ReadData());
            mainViewModel.PrintResultVM.Results.Add(data) ;
            mainViewModel.printerSerial.WriteData(data.SerialNumber);
        }

        public static TestResult GetDataResult( string scanData)
        {
            // Giả lập dữ liệu test sau khi scan QR
            var result = new TestResult
            {
                SerialNumber = "TC12501680001",
                RS_HM = "38.90",
                RS_VM = "PASS",
                LS_HM = "39.00",
                LS_VM = "PASS",
                Overall = "PASS",
                ErrorCode = "",
                DateTime = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                QRCode = scanData
            };
          ExcelHelper.AppendToExcel("test.xlsx", result);

            return result;

        }
    }
}
