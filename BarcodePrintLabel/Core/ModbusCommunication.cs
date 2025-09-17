using DocumentFormat.OpenXml.Presentation;
using NModbus;
using System;
using System.Linq;
using System.Net.Sockets;

namespace BarcodePrintLabel.Core
{
    public class ModbusCommunication
    {
        private string _strCommAddress = "127.0.0.1";
        private int _pLCPort = 502;

        public TcpClient m_modbusClient;
        private IModbusMaster _master;

        public ModbusCommunication(string pLCIPAddress, int pLCPort)
        {
            _strCommAddress = pLCIPAddress;
            _pLCPort = pLCPort;
            Connect();
        }

        public string GetConnectionString()
        {
            return $"{_strCommAddress} : {_pLCPort}";
        }

        public bool IsConnected()
        {
            return m_modbusClient != null && m_modbusClient.Connected;
        }

        public void Disconnect()
        {
            m_modbusClient?.Close();
            m_modbusClient = null;
            _master = null;
        }

        public void Connect(string pLCIPAddress = "", int pLCPort = 502)
        {
            if (!string.IsNullOrEmpty(pLCIPAddress))
            {
                _strCommAddress = pLCIPAddress;
                _pLCPort = pLCPort;
            }

            try
            {
                m_modbusClient = new TcpClient(_strCommAddress, _pLCPort);
                var factory = new ModbusFactory();
                _master = factory.CreateMaster(m_modbusClient);
            }
            catch (Exception)
            {
                // handle error
            }
        }

        public int ReadPLCRegister(byte slaveId, int address)
        {
            if (_master == null) return -1;

            lock (_master)
            {
                try
                {
                    ushort[] registers = _master.ReadHoldingRegisters(slaveId, ConvertIntToUShort(address), 1);
                    return registers[0];
                }
                catch
                {
                    return -1;
                }
            }
        }

       private static ushort ConvertIntToUShort(int value)
        {
            if (value < ushort.MinValue || value > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"Giá trị {value} không nằm trong khoảng của ushort.");
            }

            ushort result = (ushort)value;
            return result;
        }

        public int[] ReadPLCMultiRegister(byte slaveId, int startAddress, int quantity = 16)
        {

            if (_master == null) return Enumerable.Repeat(-1, quantity).ToArray();

            lock (_master)
            {
                try
                {
                    ushort[] registers = _master.ReadHoldingRegisters(slaveId, ConvertIntToUShort(startAddress), ConvertIntToUShort(quantity));
                    return registers.Select(r => (int)r).ToArray();
                }
                catch
                {
                    return Enumerable.Repeat(-1, quantity).ToArray();
                }
            }
        }

        public int WritePLCMultiRegister(byte slaveId, int startAddress, int[] values)
        {
            if (_master == null) return -1;

            lock (_master)
            {
                try
                {
                    ushort[] ushortVals = values.Select(v => (ushort)v).ToArray();
                    _master.WriteMultipleRegisters(slaveId, ConvertIntToUShort(startAddress), ushortVals);
                    return 0;
                }
                catch
                {
                    return -1;
                }
            }
        }

        public int WritePLCRegister(byte slaveId, int address, int value)
        {
            return WritePLCMultiRegister(slaveId, ConvertIntToUShort(address), new int[] { value });
        }
    }
}