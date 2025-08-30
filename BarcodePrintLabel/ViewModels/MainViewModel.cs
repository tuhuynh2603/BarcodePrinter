using BarcodePrintLabel.Core.Commands;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Communication;

namespace BarcodePrintLabel.ViewModels
{
    public class MainViewModel : BaseVM
    {
        public PrintResultDataGridViewModel PrintResultVM { get; }
        public SequenceButtonViewModel SequenceButtonsVM { get; }
        public PrinterApplication application { get; }

        public SerialCommunication printerSerial { get; set; }
        public SerialCommunication scannerSerial { get; set; }

        public ModbusCommunication modbusCommunication { get; set; }
        public ModbusCommunicationViewModel modbusCommunicationVM { get; set; }
        public SerialCommunicationViewModel printerSerialCommunicationViewModel { get; set; }
        public SerialCommunicationViewModel scanSerialCommunicationViewModel { get; set; }

        public MainViewModel()
        {
            PrintResultVM = new PrintResultDataGridViewModel(this);
            SequenceButtonsVM = new SequenceButtonViewModel(this);
            application = new PrinterApplication();
            printerSerial = new SerialCommunication(application.m_PrinterCom, application.m_PrinterBauRate);
            scannerSerial = new SerialCommunication(application.m_ScannerCom, application.m_ScannerBauRate);
            modbusCommunication = new ModbusCommunication(application.m_PLCIPAddress, application.m_PLCPort);
            modbusCommunicationVM = new ModbusCommunicationViewModel(modbusCommunication);
            printerSerialCommunicationViewModel = new SerialCommunicationViewModel(printerSerial);
            scanSerialCommunicationViewModel = new SerialCommunicationViewModel(scannerSerial);


        }

    }
}
