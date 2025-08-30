using BarcodePrintLabel.Core.Commands;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.Models;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarcodePrintLabel.ViewModels
{
    public class PrintResultDataGridViewModel : BaseVM
    {
        private MainViewModel _mainViewModel { get; set; }
        private string scanInput { get; set; }
        public string ScanInput
        {
            get => scanInput;
            set
            {
                if (scanInput != value)
                {
                    scanInput = value;
                    OnPropertyChanged(nameof(ScanInput));
                }
            }
        }

        public ObservableCollection<TestResult> Results { get; } = new ObservableCollection<TestResult>();


        public PrintResultDataGridViewModel( MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public void UpdateResultToDataGrid()
        {
            var result = AuToManualModeUtils.GetDataResult(ScanInput);
            Results.Add(result);
            ScanInput = string.Empty; // Clear input after scan
        }
    }
}
