using Amazon.Runtime.Internal.Util;
using BarcodePrintLabel.Views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace BarcodePrintLabel.Core
{
    public class PrinterApplication
    {
        public const string m_strCurrentLot_Registry = "Lot ID";
        public static string[] m_strCurrentDeviceID_Registry = { "Current Device ID 1", "Current Device ID 2" };
        public const string pathRegistry = "Software\\HD Company\\Printer Label Application";
        public string pathStatistics;

        public string m_ScannerCom;
        public int m_ScannerBauRate;
        public string m_PrinterCom;
        public int m_PrinterBauRate;
        public string m_PLCIPAddress;
        public int m_PLCPort;

        private RegistryKey register = Registry.CurrentUser.CreateSubKey(pathRegistry, true);

        public PrinterApplication()
        {
            if (!ProcessHelper.CheckMuTexProcess())
            {
                MessageBox.Show("The other Application is running!");
                ProcessHelper.KillCurrentProcess();
            }

            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            reg.SetValue("HD Printer Label Application", ToString());

            LoadRegistry();
        }

        private string GetStringRegistry(string strKey, string strDefault)
        {
            string strTemp = "";

            if ((string)register.GetValue(strKey) == "" || (string)register.GetValue(strKey) == null)
            {
                strTemp = strDefault;
                register.SetValue(strKey, strTemp);
            }
            else
                strTemp = (string)register.GetValue(strKey);

            return strTemp;
        }

        public void LoadRegistry()
        {
            #region Load Folder Lot Result Image
            pathStatistics = GetStringRegistry("Folder: Statistics", "C:\\SemiConductor Statistics");
            m_ScannerCom = GetStringRegistry("Scanner Comm", "Com1");
            int.TryParse(GetStringRegistry("Scanner Baurate", "9600"), out m_ScannerBauRate) ;

            m_PrinterCom = GetStringRegistry("Printer Comm", "Com2");
            int.TryParse(GetStringRegistry("Printer Baurate", "9600"), out m_PrinterBauRate);

            m_PLCIPAddress = GetStringRegistry("PLC IP Address", "192.168.10.1");
            int.TryParse(GetStringRegistry("PLC Port", "3000"), out m_PLCPort);

            if (!Directory.Exists(pathStatistics))
                Directory.CreateDirectory(pathStatistics);

            #endregion
        }

        public static string UserDefault;
        public static string LevelDefault;
        public static string PwsDefault;

        private string nameUserDefault;
        private string levelUserDefault;
        private string pwsUserDefault;

        public void acountDefault()
        {
            UserDefault = "Engineer";
            LevelDefault = "Engineer";
            PwsDefault = "6_XZ_VVc25>?";

            nameUserDefault = "Name=" + UserDefault;
            levelUserDefault = "Level=" + LevelDefault;
            pwsUserDefault = "Pswd=" + PwsDefault;
        }

        public static int LineNumber([System.Runtime.CompilerServices.CallerLineNumber] int lineNumber = 0)
        {
            return lineNumber;
        }
        public static string PrintCallerName()
        {
            MethodBase caller = new System.Diagnostics.StackTrace().GetFrame(1).GetMethod();
            string callerMethodName = caller.Name;
            string calledMethodName = MethodBase.GetCurrentMethod().Name;
            return $"{callerMethodName}  : {calledMethodName}";
        }
    }
}

