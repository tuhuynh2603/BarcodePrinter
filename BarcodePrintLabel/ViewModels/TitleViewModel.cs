using BarcodePrintLabel.ViewModels;
using BarcodePrintLabel.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Threading;
using DocumentFormat.OpenXml.Presentation;

namespace BarcodePrintLabel.ViewModels
{
    public class TitleViewModel
    {
        #region ICommand
        public ICommand CloseWindowCommand { get; set; }
        public ICommand MinimizeWindowCommand { get; set; }
        public ICommand MaximizeWindowCommand { get; set; }
        #endregion

        MainViewModel _mainViewModel { get; set; }
        public TitleViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            InitCommand();
        }
        public static void CloseWindow(Window w)
        {
            w.Close();
            System.Windows.Application.Current.Shutdown();
        }

        public static FrameworkElement GetWindowParent(UserControl p)
        {
            FrameworkElement parent = p;
            if (parent == null)
                return parent;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }


        private void InitCommand()
        {
            CloseWindowCommand = new RelayCommand<UserControl>((p) => { return true; },
                                                                    (p) =>
                                                                    {
                                                                        if (MessageBox.Show("Close App?", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                                                                        {
                                                                            if (_mainViewModel.modbusCommunication != null &&  _mainViewModel.modbusCommunication.IsConnected())
                                                                                _mainViewModel.modbusCommunication.Disconnect();

                                                                            if (_mainViewModel.printerPreviewDialogViewModel != null &&  _mainViewModel.printerPreviewDialogViewModel.printer.IsConnected())
                                                                                _mainViewModel.printerPreviewDialogViewModel.printer.DisConnect();

                                                                            if (_mainViewModel.scannerSerial != null && _mainViewModel.scannerSerial.IsConnected())
                                                                            {
                                                                                _mainViewModel.scannerSerial.Disconnect();
                                                                            }

                                                                            _mainViewModel.printerPreviewDialogViewModel.printer = null;
                                                                            _mainViewModel.printerPreviewDialogViewModel = null;
                                                                            _mainViewModel.scannerSerial.m_serialPort = null;
                                                                            _mainViewModel.modbusCommunication.m_modbusClient = null;
                                                                            _mainViewModel.hardwareIO = null;
                                                                            Thread.Sleep(100);
                                                                            MainWindow.mainWindow.Close();
                                                                            System.Windows.Application.Current.Shutdown();
                                                                        }
                                                                    });

            MinimizeWindowCommand = new RelayCommand<UserControl>((p) => { return true; },
                                               (p) =>
                                               {
                                                   MainWindow.mainWindow.WindowState = WindowState.Minimized;
                                               });
            MaximizeWindowCommand = new RelayCommand<UserControl>((p) => { return true; },
                                                               (p) =>
                                                               {
                                                                   if (MainWindow.mainWindow.WindowState == WindowState.Maximized)
                                                                   {
                                                                       MainWindow.mainWindow.WindowState = WindowState.Normal;
                                                                   }
                                                                   else
                                                                       MainWindow.mainWindow.WindowState = WindowState.Maximized;
                                                               });


        }
    }
}


