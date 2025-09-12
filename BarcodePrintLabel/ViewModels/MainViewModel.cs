
using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Communication;
using BarcodePrintLabel.Views;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BarcodePrintLabel.ViewModels
{
    public class MainViewModel : BaseVM
    {
        public PrintResultDataGridViewModel PrintResultVM { get; }
        public SequenceButtonViewModel SequenceButtonsVM { get; }
        public PrinterApplication application { get; }
        public SerialCommunication scannerSerial { get; set; }

        public ModbusCommunication modbusCommunication { get; set; }
        public ModbusCommunicationViewModel modbusCommunicationVM { get; set; }
        //public SerialCommunicationViewModel printerSerialCommunicationViewModel { get; set; }
        public SerialCommunicationViewModel scanSerialCommunicationViewModel { get; set; }
        public TitleViewModel titleViewModel { get; set; }
        public HardwareIO hardwareIO { get; set; }

        public ThreadManager threadManager { get; set; }


        private WindowState windowState1 = WindowState.Maximized;

        public WindowState windowState
        {
            get { return windowState1; }
            set
            {

                windowState1 = value;
                OnPropertyChanged(nameof(windowState));
            }
        }


        public LabelPreviewDialog printerPreviewDialog = new LabelPreviewDialog();
        public LabelPreviewDialogViewModel printerPreviewDialogViewModel { get; set; }

        public MainViewModel()
        {
            //         var data = new BitmapImage(
            //new Uri("pack://application:,,,/BarcodePrintLabel;component/Resources/network.png", UriKind.Absolute));
            titleViewModel =  new TitleViewModel(this);
            application = new PrinterApplication();
            printerPreviewDialogViewModel = new LabelPreviewDialogViewModel(this);

            PrintResultVM = new PrintResultDataGridViewModel(this);
            scannerSerial = new SerialCommunication(application.m_ScannerCom, application.m_ScannerBauRate, "Scanner Comm");
            modbusCommunication = new ModbusCommunication(application.m_PLCIPAddress, application.m_PLCPort);
            hardwareIO = new HardwareIO(this);
            modbusCommunicationVM = new ModbusCommunicationViewModel(modbusCommunication);
            scanSerialCommunicationViewModel = new SerialCommunicationViewModel(scannerSerial);
            SequenceButtonsVM = new SequenceButtonViewModel(this);
            threadManager = new ThreadManager(this);


        }

    }
}
