using BarcodePrintLabel.Views;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace BarcodePrintLabel.Core.Communication
{
    public class SerialCommunication
    {
        public SerialPort m_serialPort;
        public AutoResetEvent m_SerialDataReceivedEvent = new AutoResetEvent(false);
        public string strReadString { get; set; }

        private string _serialCom;
        private int _serialBaudRate;
        public string SerialTitle;

        public SerialCommunication(string strComm = "COM10", int nBaurate = 115200, string serialTitle = "")
        {
            strReadString = "";
            _serialCom = strComm;
            _serialBaudRate = nBaurate;
            SerialTitle = serialTitle;
            InitializeConnection();
            Thread ReadThread = new System.Threading.Thread(new System.Threading.ThreadStart(() => ReadThread_Fcn()));
            ReadThread.Start();
            SerialTitle = serialTitle;
        }

        public string GetConnectionString()
        {
            return $"{ _serialCom} Baurate {_serialBaudRate}";
        }
        public void InitializeConnection()
        {
            try
            {
                if (m_serialPort == null)
                {
                    m_serialPort = new SerialPort(_serialCom, _serialBaudRate, Parity.None, 8, StopBits.One);
                }
                else
                {
                    m_serialPort.Close();
                    m_serialPort = new SerialPort(_serialCom, _serialBaudRate, Parity.None, 8, StopBits.One);
                    Thread.Sleep(1000);
                }
                m_serialPort.ReadTimeout = 500;
                m_serialPort.WriteTimeout = 500;
                m_serialPort.Open();


                if (System.Windows.Application.Current == null)
                    return;


                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {

                });
            }
            catch (Exception e)
            {

                if (System.Windows.Application.Current == null)
                    return;


                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {

                });
            };

        }

        public void Disconnect()
        {
            if (m_serialPort == null)
                return;

            if (m_serialPort.IsOpen)
                m_serialPort.Close();

            if (System.Windows.Application.Current == null)
                return;


            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {

            });

        }

        public void ReadThread_Fcn()
        {
            char read;
            string strTemp = "";
            while (this != null && m_serialPort != null)
            {
                if (m_serialPort == null || m_serialPort.IsOpen == false)
                    continue;

                try
                {
                    while (m_serialPort.BytesToRead > 0)
                    {
                        read = (char)m_serialPort.ReadChar();
                        switch (read)
                        {
                            case '\r':
                                break;
                            case '\n':
                                if (strTemp.Length <= 0)
                                {
                                    continue;
                                }

                                lock (strReadString)
                                {
                                    strReadString = strTemp;
                                    strTemp = "";
                                }
                                m_SerialDataReceivedEvent.Set();

                                break;
                            default:
                                strTemp += read;
                                break;

                        }
                    }

                }
                catch (Exception e)
                {
                }
                ;

                Thread.Sleep(20);

            }
        }

        public string ReadData()
        {
            lock (strReadString)
            {
                string strTemp = strReadString;
                strReadString = "";
                return strTemp;
            }
        }

        public void WriteData(string strText)
        {
            if (!m_serialPort.IsOpen)
            {
                return;
            }

            m_serialPort.WriteLine(strText);
        }

        public bool IsConnected()
        {
            if (m_serialPort == null)
                return false;
            return m_serialPort.IsOpen;
        }

    }
}
