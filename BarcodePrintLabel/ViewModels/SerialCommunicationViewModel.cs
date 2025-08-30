using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Commands;
using BarcodePrintLabel.Core.Communication;
using Microsoft.Xaml.Behaviors.Core;
using System.Windows;
using System.Windows.Input;

namespace BarcodePrintLabel.ViewModels
{
    public class SerialCommunicationViewModel : BaseVM
    {

        private ActionCommand writeCommand;

        public ICommand WriteCommand
        {
            get
            {
                if (writeCommand == null)
                {
                    writeCommand = new ActionCommand(WriteData);
                }

                return writeCommand;
            }
        }

        private void WriteData()
        {
            _serialCommunication.WriteData(dataResult);
        }

        private ActionCommand readCommand;

        public ICommand ReadCommand
        {
            get
            {
                if (readCommand == null)
                {
                    readCommand = new ActionCommand(ReadData);
                }

                return readCommand;
            }
        }

        private void ReadData()
        {
            DataResult = _serialCommunication.ReadData();
        }

        private string dataResult;

        public string DataResult
        {
            get => dataResult;
            set
            {
                if (dataResult != value)
                {
                    dataResult = value;
                    OnPropertyChanged(nameof(DataResult));
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

        private string connectOrDisConnectString;

        public string ConnectOrDisConnectString
        {
            get => connectOrDisConnectString;
            set
            {
                if (connectOrDisConnectString != value)
                {
                    connectOrDisConnectString = value;
                    OnPropertyChanged(nameof(ConnectOrDisConnectString));
                }
            }
        }

        private Visibility _isVisible = Visibility.Collapsed;

        public Visibility IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged(nameof(IsVisible));
                }
            }
        }




        private ActionCommand connectOrDisConnect;

        public ICommand ConnectOrDisConnect
        {
            get
            {
                if (connectOrDisConnect == null)
                {
                    connectOrDisConnect = new ActionCommand(PerformConnectOrDisConnect);
                }

                return connectOrDisConnect;
            }
        }

        private void PerformConnectOrDisConnect()
        {
            if (_serialCommunication.IsConnected())
            {
                _serialCommunication.Disconnect();
            }
            else
            {
                _serialCommunication.InitializeConnection();
            }

            ConectionStatus = _serialCommunication.IsConnected();
            ConnectOrDisConnectString = ConectionStatus ? "Disconnect" : "Connect";

        }

        private SerialCommunication _serialCommunication { get; set; }

        public SerialCommunicationViewModel(SerialCommunication serial)
        {
            _serialCommunication = serial;
            Comm = _serialCommunication.GetConnectionString();
            ConectionStatus = _serialCommunication.IsConnected();
            ConnectOrDisConnectString = ConectionStatus ? "Disconnect" : "Connect";
        }

        public void OpenSerialDialog(bool isOpen)
        {
            IsVisible = isOpen ? Visibility.Visible : Visibility.Collapsed;

        }
    }
}
