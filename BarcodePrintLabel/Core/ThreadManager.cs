using BarcodePrintLabel.ConstantAndEnum;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BarcodePrintLabel.Core
{
    public class ThreadManager
    {
        public Thread thread_BarcodeReaderSequence;
        public Thread thread_HardwareIO;
        public Thread thread_TestSequence;

        MainViewModel _mainViewModel { get; set; }
        public ThreadManager(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            if (thread_HardwareIO == null)
            {
                thread_HardwareIO = new Thread(new ThreadStart(mainViewModel.hardwareIO.ReadPLCIO));
                thread_HardwareIO.Start();
                }
            else if (!thread_HardwareIO.IsAlive)
            {
                thread_HardwareIO = new Thread(new ThreadStart(mainViewModel.hardwareIO.ReadPLCIO));
                thread_HardwareIO.Start();
            }
            // sequence
            if (thread_BarcodeReaderSequence == null)
            {
                thread_BarcodeReaderSequence = new Thread(() => AuToManualModeUtils.DoAutoMode(_mainViewModel));
                thread_BarcodeReaderSequence.Start();
            }
            else if (!thread_BarcodeReaderSequence.IsAlive)
            {
                thread_BarcodeReaderSequence = new Thread(() => AuToManualModeUtils.DoAutoMode(_mainViewModel));
                thread_BarcodeReaderSequence.Start();
            }
        }
    }
}
