using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Services;
using Microsoft.Windows.Input;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ZXing;
using ZXing.Common;
using ZXing.Windows.Compatibility;

namespace BarcodePrintLabel.ViewModels
{
    public class LabelPreviewDialogViewModel :BaseVM
    {

        private MainViewModel _mainViewModel { get; set; }
        public ObservableCollection<string> LabelTypes { get; } =
        new ObservableCollection<string> { "Unit" };
        public ZebraPrinterService printer;


        public string SelectedLabelType { get; set; } = "Unit";

        private string _SerialNumber = "12345678";
        public string SerialNumber
        {
            get => _SerialNumber;
            set
            {
                _SerialNumber = value;
                OnPropertyChanged();
                GenerateBarcode();
            }
        }

        private string _SerialTitle = "Serial Comm";

        public string SerialTitle
        {
            get => _SerialTitle;
            set
            {
                if (_SerialTitle != value)
                {
                    _SerialTitle = value;
                    OnPropertyChanged(nameof(SerialTitle));
                }
            }
        }


        private bool conectionStatus;

        public bool ConectionStatus
        {
            get => conectionStatus;
            set
            {
                if (conectionStatus != value)
                {
                    conectionStatus = value;
                    OnPropertyChanged(nameof(ConectionStatus));
                }
            }
        }

        private string _comm;

        public string Comm
        {
            get => _comm;
            set
            {
                if (_comm != value)
                {
                    _comm = value;
                    OnPropertyChanged(nameof(Comm));
                }
            }
        }

        private WriteableBitmap _barcodeImage;
        public WriteableBitmap BarcodeImage
        {
            get => _barcodeImage;
            set { _barcodeImage = value; OnPropertyChanged(); }
        }
        public ICommand PreviewCommand { get; }   // 👈 thêm PreviewCommand

        public ICommand PrintCommand { get; }

        public LabelPreviewDialogViewModel(MainViewModel mainViewModel)
        {
            PrintCommand = new ActionCommand(Print);
            PreviewCommand = new ActionCommand(GenerateBarcode);   // 👈 gán PreviewCommand
            _mainViewModel = mainViewModel;
            printer = new ZebraPrinterService(_mainViewModel.application.m_PrinterCom, _mainViewModel.application.m_PrinterBauRate);
            conectionStatus = printer.IsConnected();
            Comm = "Printer: "+  printer.GetConnectionString();
            GenerateBarcode();
        }

        private void GenerateBarcode()
        {
            if (string.IsNullOrEmpty(SerialNumber))
            {
                BarcodeImage = null;
                return;
            }

            var writer = new BarcodeWriter<WriteableBitmap>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 80,
                    Width = 400,
                    Margin = 2
                },
                Renderer = new WriteableBitmapRenderer()
            };

            BarcodeImage = writer.Write(SerialNumber);
        }

        public void Print( string serialNumber, string weekOfYear)
        {
            SerialNumber = serialNumber;
            printer.PrintUnitLabel(SerialNumber, weekOfYear);

            //switch (SelectedLabelType)
            //{
            //    case "Unit":
            //        printer.PrintUnitLabel(Barcode, DateCode);
            //        break;
            //    case "Box":
            //        printer.PrintBoxLabel(Model, ProductCode, DateCode, Quantity, Barcode);
            //        break;
            //    case "Carton":
            //        printer.PrintCartonLabel(Model, "Flushvalve Electronic Operator",
            //                                 Quantity.ToString(), "1/50", Barcode);
            //        break;
            //    case "Full":
            //        printer.PrintFullLabel("SLOAN.GRF", Model, ProductCode, DateCode, Quantity, Barcode);
            //        break;
            //}
        }

        private void Print()
        {
            var DateCode = ProcessHelper.GetWeekOfYear(DateTime.Now);
            printer.PrintUnitLabel(SerialNumber, DateCode);

            //switch (SelectedLabelType)
            //{
            //    case "Unit":
            //        printer.PrintUnitLabel(Barcode, DateCode);
            //        break;
            //    case "Box":
            //        printer.PrintBoxLabel(Model, ProductCode, DateCode, Quantity, Barcode);
            //        break;
            //    case "Carton":
            //        printer.PrintCartonLabel(Model, "Flushvalve Electronic Operator",
            //                                 Quantity.ToString(), "1/50", Barcode);
            //        break;
            //    case "Full":
            //        printer.PrintFullLabel("SLOAN.GRF", Model, ProductCode, DateCode, Quantity, Barcode);
            //        break;
            //}
        }

    }
}
