using EasyModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            lock (m_modbusClient)
            {
                int brepeat = 0;
                RetryRead:
                Thread.Sleep(10);
                if (m_modbusClient.Connected)
                    try
                    {
                        int[] a = new int[10];

                        a = m_modbusClient.ReadHoldingRegisters(nAddress, 5);

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
                            goto RetryRead;
                        }
                        int a = m_modbusClient.ConnectionTimeout;

                        goto RetryRead;
                    }

                else
                    return -1;
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

                        int a = m_modbusClient.ConnectionTimeout;

                        goto RetryWrite;
                    }
                }
                else
                    return -1;
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

                        int a = m_modbusClient.ConnectionTimeout;

                        goto RetryWrite;
                    }
                }
                else
                    return -1;
                return 0;
            }
        }
    }
}
