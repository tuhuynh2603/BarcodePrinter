using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.ViewModels;
using ClosedXML.Excel;
using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BarcodePrintLabel.Core
{
    public class HardwareIO
    {
        public bool IsByPass { get; set; } = false;
        public bool IsAutoMode { get; set; } = false;

        private MainViewModel _mainViewModel { get; set; }

        private ModbusCommunication _modbusCommunication { get; set; }

        public HardwareIO(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _modbusCommunication = _mainViewModel.modbusCommunication;
        }

        private void WriteDateTimeToPLC( int startAddress)
        {
            var eventTimeCount = _modbusCommunication.ReadPLCRegister(1, startAddress); // Test connection  to avoid delay when read first time
            var YMDHMS = GetDateTimeArray();
            _modbusCommunication.WritePLCMultiRegister(1, startAddress + 1, YMDHMS);
            _modbusCommunication.WritePLCRegister(1, startAddress, eventTimeCount + 1);
        }

        private static int[] GetDateTimeArray()
        {
            var YMDHMS = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            var dateTimeArray = YMDHMS
                .Split('-')
                .Select(s =>
                {
                    int.TryParse(s, out int number);
                    return number;
                })
                .ToArray();
            return dateTimeArray;
        }

        public async void ReadPLCIO()
        {
            var eventCount = _mainViewModel.application.m_PLCEventCount;
            var numberRegister = _mainViewModel.application.m_DefaultScanDataLength + _mainViewModel.application.m_DefaultPLCResultLength;
            var PLC_EVENT = (int)PLC_ADDRESS.APP_SCAN_RESULT + _mainViewModel.application.m_DefaultScanDataLength;
            var PLC_RESULT = (int)PLC_EVENT + 1;

            WriteDateTimeToPLC(_mainViewModel.application.m_PLCEventTimeAddress);
            while (_mainViewModel.hardwareIO != null)
            {
                System.Windows.Application.Current?.Dispatcher.Invoke((Action)delegate
                {
                    _mainViewModel.printerPreviewDialogViewModel.ConectionStatus = _mainViewModel.printerPreviewDialogViewModel.printer.IsConnected();
                    _mainViewModel.modbusCommunicationVM.ConectionStatus = _mainViewModel.modbusCommunication.IsConnected();

                });

                if (_modbusCommunication?.m_modbusClient is null || !_modbusCommunication.IsConnected() )
                {
                    Thread.Sleep(500);
                    continue;
                }

                
                var plcEventCount = _modbusCommunication.ReadPLCRegister( 1, PLC_EVENT);
                if( plcEventCount == eventCount )
                {
                    Thread.Sleep(500);
                    continue;
                }

                var plcTestResult = _modbusCommunication.ReadPLCMultiRegister( 1, PLC_RESULT, numberRegister);
                var data = AuToManualModeUtils.GetDataResult( _mainViewModel.PrintResultVM.Results, plcTestResult, _mainViewModel.application.m_DefaultScanDataLength);
                _mainViewModel.PrintResultVM.Results.Add(data);
                var path = $"{_mainViewModel.application.pathStatistics}\\{_mainViewModel.application.defaultExcelFile}";

                eventCount = plcEventCount + 1;
                _modbusCommunication.WritePLCRegister( 1, PLC_EVENT, eventCount);
                _mainViewModel.application.SetDataToRegistry(_mainViewModel.application.PLCEventCountRegistryKey, eventCount.ToString());

                await _mainViewModel.PrintResultVM.AddDataToSQL(data);
                //ExcelHelper.AppendToExcel(path, data);
                if (AuToManualModeUtils.ShouldPrintData(_mainViewModel.hardwareIO.IsByPass, data, _mainViewModel.hardwareIO.IsAutoMode))
                {
                    _mainViewModel.printerPreviewDialogViewModel.Print(data.SerialNumber, ProcessHelper.GetWeekOfYear(data.DateTime));
                }

                System.Windows.Application.Current?.Dispatcher.Invoke((Action)delegate
                {
                    _mainViewModel.PrintResultVM.Results.Add(data);
                    if (data.RESULT == AuToManualModeUtils.PASS_RESULT_CODE)
                        _mainViewModel.PrintResultVM.ResultsDisplay.Add(data);
                });

                Thread.Sleep(500);
            }
        }

        public async void ReadPLCIOTest()
        {
            var eventCount = _mainViewModel.application.m_PLCEventCount;
            var numberRegister = _mainViewModel.application.m_DefaultScanDataLength + _mainViewModel.application.m_DefaultPLCResultLength;
            var PLC_EVENT = (int)PLC_ADDRESS.APP_SCAN_RESULT + _mainViewModel.application.m_DefaultScanDataLength;
            var PLC_RESULT = (int)PLC_EVENT + 1;
            while ( _mainViewModel != null && _mainViewModel.hardwareIO != null && _mainViewModel.SequenceButtonsVM.IsOpenModbusDialog)
            {

                var data = TestScenarios.GetDataResult(_mainViewModel.PrintResultVM.Results, _mainViewModel.application.m_DefaultScanDataLength);
                var path = $"{_mainViewModel.application.pathStatistics}\\{_mainViewModel.application.defaultExcelFile}";

                _modbusCommunication.WritePLCRegister(1, PLC_EVENT, eventCount);
                _mainViewModel.application.SetDataToRegistry(_mainViewModel.application.PLCEventCountRegistryKey, eventCount.ToString());

                await _mainViewModel.PrintResultVM.AddDataToSQL(data);
                //ExcelHelper.AppendToExcel(path, data);
                if (AuToManualModeUtils.ShouldPrintData(_mainViewModel.hardwareIO.IsByPass, data, _mainViewModel.hardwareIO.IsAutoMode))
                {
                    _mainViewModel.printerPreviewDialogViewModel.Print(data.SerialNumber, ProcessHelper.GetWeekOfYear(data.DateTime));
                }

                System.Windows.Application.Current?.Dispatcher.Invoke((Action)delegate
                {
                    _mainViewModel.PrintResultVM.Results.Add(data);
                    if(data.RESULT == AuToManualModeUtils.PASS_RESULT_CODE)
                        _mainViewModel.PrintResultVM.ResultsDisplay.Add(data);
                });

                Thread.Sleep(2000);
            }
        }

    }
}
