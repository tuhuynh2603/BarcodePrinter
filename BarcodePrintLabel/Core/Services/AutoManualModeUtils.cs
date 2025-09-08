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

    public static class AuToManualModeUtils
    {
        public const string PASS_RESULT_CODE = "0";
        public static void DoAutoMode(MainViewModel mainViewModel)
        {
            while (true)
            {
                while (mainViewModel.printerSerial != null && !mainViewModel.scannerSerial.m_SerialDataReceivedEvent.WaitOne(100))
                {
                    Thread.Sleep(100);
                }
                if (mainViewModel.printerSerial is null)
                    return;

                var scannerData = mainViewModel.scannerSerial.ReadData();
                System.Windows.Application.Current?.Dispatcher.Invoke((Action)delegate
                {
                    mainViewModel.PrintResultVM.ScannerData = scannerData;
                });

                if (scannerData.Length != mainViewModel.application.m_DefaultScanDataLength)
                {
                    MessageBox.Show($"Scan data is invalid. Please check again. Data: {scannerData}", "Data Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    continue;
                }

                var scannerDataAsciiArray = scannerData.Select(c => (int)c).ToArray();
                mainViewModel.modbusCommunication.WritePLCMultiRegister((int)PLC_ADDRESS.APP_SCAN_RESULT, scannerDataAsciiArray);
                Thread.Sleep(2000);
            }
        }

        public static void PrintLabel( SerialCommunication printerSerial, bool isByPass, TestResult result, bool isAutoMode)
        {
            if (ShouldPrintData(isByPass, result, isAutoMode) )
            {
                var command = PrinterTemplate.MakePrinterTemplateCommand(result.SerialNumber);
                printerSerial.WriteData(command);
            }
        }

        public static bool ShouldPrintData( bool isByPass, TestResult result, bool isAutoMode)
        {
            return result.ERROR_CODE == "0";
        }

        public static TestResult GetDataResult( IReadOnlyCollection<TestResult> allDataResults, int[] plcTestResult, int defaultScanDataLength)
        {
            var now = DateTime.Now;
            var scanData = new string(plcTestResult.Take(defaultScanDataLength).Select(x => (char)x).ToArray());
            // Giả lập dữ liệu test sau khi scan QR
            var dataTest = plcTestResult.Skip(defaultScanDataLength ).ToList();

            var result = new TestResult
            {
                QRCode = scanData,
                LS_HORIZONTAL = dataTest[0].ToString(),
                LS_VERTICAL = dataTest[1].ToString(),
                LS_HORIZONTAL2 = dataTest[2].ToString(),
                LS_VERTICAL2 = dataTest[3].ToString(),
                RS_HORIZONTAL = dataTest[4].ToString(),
                RS_VERTICAL = dataTest[5].ToString(),
                RS_HORIZONTAL2 = dataTest[6].ToString(),
                RS_VERTICAL2 = dataTest[7].ToString(),
                RESULT = dataTest[8].ToString(),
                ERROR_CODE = dataTest[9].ToString(),
                DateTime = now.ToString(),

            };

            result.SerialNumber = GenerateSerialNumber( allDataResults, result, now);

            return result;
        }

        private static string GenerateSerialNumber( IReadOnlyCollection<TestResult> allData, TestResult result, DateTime now)
        {
            const string BN = "BN";
            const string TC = "TC";
            var qrCode = result.QRCode;
            var data = allData.Where(s => s.QRCode == qrCode);
            if (data.Count() > 0 )
            {
                return data.First().SerialNumber;
            }

            if (result.ERROR_CODE != PASS_RESULT_CODE)
                return string.Empty;

            var manufacture = qrCode.Substring(0, 2);
            var isByPass = manufacture.Contains("BN");
            var testNumber = isByPass ? "1" : data.Count().ToString();// if BN => always = 1
            var year = now.ToString("yy");
            var dateOfYear = now.DayOfYear.ToString("D3");
            var YearMonthDay = now.ToString();
            var allPassDataInDate = allData.Where(s => s.ERROR_CODE == PASS_RESULT_CODE && DateTime.Parse(s.DateTime).Date == now.Date );
            var BNDataInDate = allPassDataInDate.Where(s => s.SerialNumber.Contains(BN)).Count();
            var TCDataInDate = allPassDataInDate.Where(s=> s.SerialNumber.Contains(TC)).DistinctBy(s => s.QRCode).Count();
            var sequentialNumber = (BNDataInDate + TCDataInDate + 1).ToString("D4");
            // perday 
            // sequentialNumber = so luong pass BN + co scan QR unique ID
            return $"{manufacture}{testNumber}{year}{dateOfYear}{sequentialNumber}";// string.Join(manufacture, testNumber, year, dateOfYear, sequentialNumber);
            //
        }
    }

    public static class LinqExtensions
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
