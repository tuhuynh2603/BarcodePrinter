using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Communication;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.Models;
using BarcodePrintLabel.Views;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BarcodePrintLabel.ViewModels
{
    public class SequenceButtonViewModel : BaseVM
    {
        private MainViewModel _mainViewModel { get; set; }
        private bool _isByPass { get; set; }
        public bool IsByPass
        {
            get => _isByPass;
            set
            {
                if (_isByPass != value)
                {
                    _isByPass = value;
                    OnPropertyChanged(nameof(IsByPass));
                }
            }
        }

        private bool _isAutoMode { get; set; }
        public bool IsAutoMode
        {
            get => _isAutoMode;
            set
            {
                if (_isAutoMode != value)
                {
                    _isAutoMode = value;
                    OnPropertyChanged(nameof(IsAutoMode));
                }

                //if(IsAutoMode)
                //    StartAutoMode();
            }
        }

        //private bool _isManualMode { get; set; }
        //public bool IsManualMode
        //{
        //    get => _isManualMode;
        //    set
        //    {
        //        if (_isManualMode != value)
        //        {
        //            _isManualMode = value;
        //            OnPropertyChanged(nameof(IsManualMode));
        //        }

        //        if(IsManualMode)
        //            StartManualMode();
        //    }
        //}

        //private void StartAutoMode()
        //{
        //    IsManualMode = false; // Ensure manual mode is off when starting auto mode

        //    Thread ReadThread = new Thread(new ThreadStart(ReadPLCIO));

        //    AuToManualModeUtils.StartSequence(PrintMode.AutoMode, _mainViewModel);
        //}

        private void StartManualMode()
        {
            IsAutoMode = false;
            //AuToManualModeUtils.StartSequence(PrintMode.ManualMode, _mainViewModel);
        }

        private ActionCommand _PrintLabelCommand;

        public ICommand PrintLabelCommand
        {
            get
            {
                if (_PrintLabelCommand == null)
                {
                    _PrintLabelCommand = new ActionCommand(PrintLabel);
                }

                return _PrintLabelCommand;
            }
        }

        private void PrintLabel()
        {
            if (_mainViewModel.PrintResultVM.Results.Count == 0)
                return;

            if(_mainViewModel.PrintResultVM?.SelectedResult != null)
            {
                var data = _mainViewModel.PrintResultVM.SelectedResult;
                _mainViewModel.printerPreviewDialogViewModel.Print(data.SerialNumber, ProcessHelper.GetWeekOfYear(data.DateTime));
            }
            else
            {
                var data = _mainViewModel.PrintResultVM.Results.Last() ;
                _mainViewModel.printerPreviewDialogViewModel.Print(data.SerialNumber, ProcessHelper.GetWeekOfYear(data.DateTime));
            }
        }
        
        public SequenceButtonViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            
        }

        private bool _isOpenModbusDialog { get; set; } = true;
        public bool IsOpenModbusDialog
        {
            get => _isOpenModbusDialog;
            set
            {
                _isOpenModbusDialog = value = true;
               
                if (_isOpenModbusDialog != value)
                {
                    _isOpenModbusDialog = value;
                    OnPropertyChanged(nameof(IsOpenModbusDialog));
                }

                //if(_mainViewModel.modbusCommunicationVM != null)
                //    _mainViewModel.modbusCommunicationVM.OpenModbusDialog(_isOpenModbusDialog);

                //if (_isOpenModbusDialog)
                //{

                //    if (_mainViewModel.threadManager.thread_TestSequence == null)
                //    {
                //        _mainViewModel.threadManager.thread_TestSequence = new Thread(() => _mainViewModel.hardwareIO.ReadPLCIOTest());
                //        _mainViewModel.threadManager.thread_TestSequence.Start();
                //    }
                //    else if (!_mainViewModel.threadManager.thread_TestSequence.IsAlive)
                //    {
                //        _mainViewModel.threadManager.thread_TestSequence = new Thread(() => _mainViewModel.hardwareIO.ReadPLCIOTest());
                //        _mainViewModel.threadManager.thread_TestSequence.Start();
                //    }
                //}
            }
        }

        private bool _isOpenPrinterCommunicationDialog { get; set; } = true;
        public bool IsOpenPrinterCommunicationDialog
        {
            get => _isOpenPrinterCommunicationDialog;
            set
            {
                //if(_mainViewModel.application.IsEngineerMode == 1)

                _isOpenModbusDialog = value = true;
                if (_isOpenPrinterCommunicationDialog != value)
                {
                    _isOpenPrinterCommunicationDialog = value;
                    OnPropertyChanged(nameof(IsOpenPrinterCommunicationDialog));
                }
                //if(_isOpenPrinterCommunicationDialog)
                //    _mainViewModel.printerPreviewDialog.Visibility = Visibility.Visible;
                //else
                //    _mainViewModel.printerPreviewDialog.Visibility = Visibility.Collapsed;

                //if (_mainViewModel.printerSerialCommunicationViewModel != null)
                //    _mainViewModel.printerSerialCommunicationViewModel.OpenSerialDialog(_isOpenPrinterCommunicationDialog);
            }
        }

        private bool _isOpenScannerCommunicationDialog { get; set; } = true;
        public bool IsOpenScannerCommunicationDialog
        {
            get => _isOpenScannerCommunicationDialog;
            set
            {
                _isOpenModbusDialog = value = true;
                if (_isOpenScannerCommunicationDialog != value)
                {
                    _isOpenScannerCommunicationDialog = value;
                    OnPropertyChanged(nameof(IsOpenScannerCommunicationDialog));
                }

                //if (_mainViewModel.scanSerialCommunicationViewModel != null)
                //    _mainViewModel.scanSerialCommunicationViewModel.OpenSerialDialog(_isOpenScannerCommunicationDialog);
            }
        }

    }
}
