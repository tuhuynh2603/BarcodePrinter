using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Commands;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BarcodePrintLabel.ViewModels
{
    public class ModbusCommunicationViewModel : BaseVM
    {

        private ActionCommand writeModbusCommand;

        public ICommand WriteModbusCommand
        {
            get
            {
                if (writeModbusCommand == null)
                {
                    writeModbusCommand = new ActionCommand(WriteModbus);
                }

                return writeModbusCommand;
            }
        }

        private void WriteModbus()
        {
            var memoryAddress = modbusMemoryAddress;
            try
            {
                memoryAddress = Convert.ToInt32(ModbusMemoryAddress);
                var value = Convert.ToInt32(modbusValue);
                _modbusCommunication.WritePLCRegister(memoryAddress, value);
            }
            catch (Exception ex)
            {
                modbusValue = ex.ToString();
                // Handle conversion error if necessary
                return;
            }
        }

        private ActionCommand readModbusCommand;

        public ICommand ReadModbusCommand
        {
            get
            {
                if (readModbusCommand == null)
                {
                    readModbusCommand = new ActionCommand(ReadModbus);
                }

                return readModbusCommand;
            }
        }

        private void ReadModbus()
        {
            var memoryAddress = modbusMemoryAddress;
            try
            {
                memoryAddress = Convert.ToInt32(ModbusMemoryAddress);
                modbusValue = _modbusCommunication.ReadPLCRegister(memoryAddress).ToString();
            }
            catch (Exception ex)
            {
                modbusValue = ex.ToString();
                // Handle conversion error if necessary
                return;
            }
        }

        private int modbusMemoryAddress;

        public int ModbusMemoryAddress
        {
            get => modbusMemoryAddress;
            set
            {
                if (modbusMemoryAddress != value)
                {
                    modbusMemoryAddress = value;
                    OnPropertyChanged(nameof(ModbusMemoryAddress));
                }
            }
        }

        private string modbusValue;

        public string ModbusValue
        {
            get => modbusValue;
            set
            {
                if (modbusValue != value)
                {
                    modbusValue = value;
                    OnPropertyChanged(nameof(ModbusValue));
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

        private string iPAddress;

        public string ModbusIPAddress
        {
            get => iPAddress;
            set
            {
                if (iPAddress != value)
                {
                    iPAddress = value;
                    OnPropertyChanged(nameof(ModbusIPAddress));
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




        private ActionCommand connectOrDisConnectToModbus;

        public ICommand ConnectOrDisConnectToModbus
        {
            get
            {
                if (connectOrDisConnectToModbus == null)
                {
                    connectOrDisConnectToModbus = new ActionCommand(PerformConnectOrDisConnectToModbus);
                }

                return connectOrDisConnectToModbus;
            }
        }

        private void PerformConnectOrDisConnectToModbus(object commandParameter)
        {
            if(_modbusCommunication.IsConnected())
            {
                _modbusCommunication.Disconnect();
            }
            else
            {
                _modbusCommunication.Connect();
            }

            ConnectOrDisConnectString = ConectionStatus ? "Disconnect" : "Connect";

        }

        private ModbusCommunication _modbusCommunication { get; set; }

        public ModbusCommunicationViewModel( ModbusCommunication modbusCommunication)
        {
            _modbusCommunication = modbusCommunication;
            ModbusIPAddress = _modbusCommunication.GetConnectionString();
            ConectionStatus = _modbusCommunication.IsConnected();
            ConnectOrDisConnectString = ConectionStatus ? "Disconnect" : "Connect";
        }

        public void OpenModbusDialog(bool isOpenModbusDialog)
        {
           IsVisible = isOpenModbusDialog ? Visibility.Visible :Visibility.Collapsed;

        }
    }
}
