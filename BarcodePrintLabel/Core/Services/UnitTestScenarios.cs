using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Core.Communication;
using BarcodePrintLabel.Models;
using BarcodePrintLabel.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BarcodePrintLabel.Core.Services
{

    public static class TestScenarios
    {
        public const string PASS_RESULT_CODE = "0";

        public static void PrintLabel(SerialCommunication printerSerial, bool isByPass, TestResult result, bool isAutoMode)
        {
            if (ShouldPrintData(isByPass, result, isAutoMode))
            {
                printerSerial.WriteData(result.SerialNumber);
            }
        }

        public static bool ShouldPrintData(bool isByPass, TestResult result, bool isAutoMode)
        {
            return result.ERROR_CODE == "0";
        }

        public static TestResult GetDataResult(IReadOnlyCollection<TestResult> allDataResults, int defaultScanDataLength)
        {
            //if (plcTestResult.Length < 20)
            //{
            //    MessageBox.Show($"PLC data is invalid {plcTestResult}. Please check again.", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            Random rnd = new Random();

            var now = DateTime.Now;
            var scanData = $"BN25001-000000{rnd.Next(1, 11)}";
            // Giả lập dữ liệu test sau khi scan QR

            var result = new TestResult
            {
                QRCode = scanData,
                LS_HORIZONTAL = "1",
                LS_VERTICAL ="1",
                LS_HORIZONTAL2 = "1",
                LS_VERTICAL2 = "1",
                RS_HORIZONTAL = "1",
                RS_VERTICAL = "1",
                RS_HORIZONTAL2 = "1",
                RS_VERTICAL2 = "1",
                RESULT = "1",
                ERROR_CODE = rnd.Next(0, 1).ToString(),
                DateTime = now,
            };

            result.SerialNumber = GenerateSerialNumber(allDataResults, result, now);

            return result;
        }

        private static string GenerateSerialNumber(IReadOnlyCollection<TestResult> allData, TestResult result, DateTime now)
        {
            const string BN = "BN";
            const string TC = "TC";
            var qrCode = result.QRCode;
            var data = allData.Where(s => s.QRCode == qrCode);


            var manufacture = qrCode.Substring(0, 2);
            var isByPass = manufacture.Contains("BN");
            if (data.Count() > 0 && !isByPass)
            {
                return data.First().SerialNumber;
            }

            if (result.ERROR_CODE != PASS_RESULT_CODE)
                return string.Empty;
            var testNumber = isByPass ? "1" : data.Count().ToString();// if BN => always = 1
            var year = now.ToString("yy");
            var dateOfYear = now.DayOfYear.ToString("D3");
            var YearMonthDay = now.ToString();
            var allPassDataInDate = allData.Where(s => s.ERROR_CODE == PASS_RESULT_CODE && s.DateTime.Date == now.Date);
            var BNDataInDate = allPassDataInDate.Where(s => s.SerialNumber.Contains(BN)).Count();
            var TCDataInDate = allPassDataInDate.Where(s => s.SerialNumber.Contains(TC)).DistinctBy(s => s.QRCode).Count();
            var sequentialNumber = (BNDataInDate + TCDataInDate + 1).ToString("D4");
            // perday 
            // sequentialNumber = so luong pass BN + co scan QR unique ID
            return $"{manufacture}{year}{dateOfYear}{testNumber}{sequentialNumber}";
            //
        }
    }
}
