using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Core.Services;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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

                if(IsAutoMode)
                    StartAutoMode();
            }
        }

        private bool _isManualMode { get; set; }
        public bool IsManualMode
        {
            get => _isManualMode;
            set
            {
                if (_isManualMode != value)
                {
                    _isManualMode = value;
                    OnPropertyChanged(nameof(IsManualMode));
                }

                if(IsManualMode)
                    StartManualMode();
            }
        }

        private void StartAutoMode()
        {
            IsManualMode = false; // Ensure manual mode is off when starting auto mode
            AuToManualModeUtils.StartSequence(PrintMode.AutoMode, _mainViewModel);
        }

        private void StartManualMode()
        {
            IsAutoMode = false;
            AuToManualModeUtils.StartSequence(PrintMode.ManualMode, _mainViewModel);
        }

        public SequenceButtonViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        private bool _isOpenModbusDialog { get; set; }
        public bool IsOpenModbusDialog
        {
            get => _isOpenModbusDialog;
            set
            {
                if (_isOpenModbusDialog != value)
                {
                    _isOpenModbusDialog = value;
                    OnPropertyChanged(nameof(IsOpenModbusDialog));
                }

                if(_mainViewModel.modbusCommunicationVM != null)
                    _mainViewModel.modbusCommunicationVM.OpenModbusDialog(_isOpenModbusDialog);
            }
        }

        private bool _isOpenPrinterCommunicationDialog { get; set; }
        public bool IsOpenPrinterCommunicationDialog
        {
            get => _isOpenPrinterCommunicationDialog;
            set
            {
                if (_isOpenPrinterCommunicationDialog != value)
                {
                    _isOpenPrinterCommunicationDialog = value;
                    OnPropertyChanged(nameof(IsOpenPrinterCommunicationDialog));
                }

                if (_mainViewModel.printerSerialCommunicationViewModel != null)
                    _mainViewModel.printerSerialCommunicationViewModel.OpenSerialDialog(_isOpenPrinterCommunicationDialog);
            }
        }

        private bool _isOpenScannerCommunicationDialog { get; set; }
        public bool IsOpenScannerCommunicationDialog
        {
            get => _isOpenScannerCommunicationDialog;
            set
            {
                if (_isOpenScannerCommunicationDialog != value)
                {
                    _isOpenScannerCommunicationDialog = value;
                    OnPropertyChanged(nameof(IsOpenScannerCommunicationDialog));
                }

                if (_mainViewModel.scanSerialCommunicationViewModel != null)
                    _mainViewModel.scanSerialCommunicationViewModel.OpenSerialDialog(_isOpenScannerCommunicationDialog);
            }
        }

    }
}
