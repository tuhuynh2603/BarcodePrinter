using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BarcodePrintLabel.Core
{
    public class ModbusCommunication
    {
        public ModbusClient m_modbusClient;
        private string _strCommAddress = "127.0.0.1";
        private int _pLCPort  = 502;
        public const string DisconnectedString = "Disconnected";
        public ModbusCommunication(string pLCIPAddress, int pLCPort)
        {
            _strCommAddress = pLCIPAddress;
            _pLCPort = pLCPort;
            m_modbusClient = new ModbusClient(_strCommAddress, _pLCPort);
            m_modbusClient.ConnectionTimeout = 2000;
            try
            {
                m_modbusClient.Connect();
            }
            catch (Exception e)
            {
                // Handle connection error if necessary
            }
        }

        public string GetConnectionString()
        {
            return $"{_strCommAddress} : {_pLCPort}";
        }

        public bool IsConnected()
        {
            return m_modbusClient.Connected;
        }

        public void Disconnect()
        {
            if (m_modbusClient.Connected)
            {
                m_modbusClient.Disconnect();
            }
        }

        public void Connect(string pLCIPAddress = "", int pLCPort = 502)
        {
            if (!string.IsNullOrEmpty(pLCIPAddress))
            {
                _strCommAddress = pLCIPAddress;
                _pLCPort = pLCPort;
            }

            if (!m_modbusClient.Connected)
            {
                try
                {
                    m_modbusClient.Connect(_strCommAddress, _pLCPort);
                }
                catch (Exception e)
                {
                }
            }
        }

        public int ReadPLCRegister(int nAddress)
        {
            if (m_modbusClient == null)
                return -1;

            lock (m_modbusClient)
            {
                int brepeat = 0;
                RetryRead:
                Thread.Sleep(10);
                if (m_modbusClient.Connected)
                    try
                    {
                        int[] a = new int[16];

                        a = m_modbusClient.ReadHoldingRegisters(nAddress, a.Count());

                        return a[0];
                    }

                    catch
                    {
                        brepeat++;
                        Thread.Sleep(10);
                        if (brepeat > 10)
                            return -1;
                        try
                        {
                            m_modbusClient.Disconnect();
                            m_modbusClient.Connect();
                        }
                        catch
                        {
                        }

                        goto RetryRead;
                    }

                else
                {
                    return -1;
                }
            }
        }

        public int[] ReadPLCMultiRegister(int nAddress, int quantity = 16)
        {
            lock (m_modbusClient)
            {
                 var data = new int[quantity];
                var failedData = data.Select(s => -1).ToArray();

                int brepeat = 0;
                RetryRead:
                Thread.Sleep(10);
                if (m_modbusClient.Connected)
                {
                    try
                    {
                        data = m_modbusClient.ReadHoldingRegisters(nAddress, quantity);
                        return data;
                    }

                    catch
                    {
                        brepeat++;
                        Thread.Sleep(10);
                        if (brepeat > 10)
                            return failedData;
                        try
                        {
                            m_modbusClient.Disconnect();
                            m_modbusClient.Connect();
                        }
                        catch
                        {
                            goto RetryRead;
                        }

                        goto RetryRead;
                    }
                }
                else
                {
                    //MessageBox.Show("PLC Disconnected. Please check the connection.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return failedData;
                }
            }
        }

        public int WritePLCMultiRegister(int nAddress, int[] ival)
        {
            lock (m_modbusClient)
            {
                int brepeat = 0;
                RetryWrite:
                Thread.Sleep(10);
                if (m_modbusClient.Connected)
                {
                    try
                    {
                        m_modbusClient.WriteMultipleRegisters(nAddress, ival);
                        Thread.Sleep(10);
                    }
                    catch (Exception e)
                    {
                        brepeat++;
                        Thread.Sleep(10);
                        if (brepeat > 10)
                            return -1;
                        try
                        {
                            m_modbusClient.Disconnect();
                            m_modbusClient.Connect();
                        }
                        catch
                        {
                            goto RetryWrite;
                        }

                        goto RetryWrite;
                    }
                }
                else
                {
                    //MessageBox.Show("PLC Disconnected. Please check the connection.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
                return 0;
            }
        }

        public int WritePLCRegister(int nAddress, int nValue)
        {
            lock (m_modbusClient)
            {
                int[] ival = new int[1];
                ival[0] = nValue;
                int brepeat = 0;
                RetryWrite:
                Thread.Sleep(10);
                if (m_modbusClient.Connected)
                {
                    try
                    {
                        m_modbusClient.WriteMultipleRegisters(nAddress, ival);
                        Thread.Sleep(10);
                    }
                    catch (Exception e)
                    {
                        brepeat++;
                        Thread.Sleep(10);
                        if (brepeat > 10)
                            return -1;
                        try
                        {
                            m_modbusClient.Disconnect();
                            m_modbusClient.Connect();
                        }
                        catch
                        {
                            goto RetryWrite;
                        }

                        goto RetryWrite;
                    }
                }
                else
                {
                    //MessageBox.Show("PLC Disconnected. Please check the connection.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return -1;
                }
                return 0;
            }
        }
    }
}
