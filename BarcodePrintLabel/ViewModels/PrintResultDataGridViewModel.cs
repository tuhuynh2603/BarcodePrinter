using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Commands;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.Models;
using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

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

        private string _ScannerData { get; set; }
        public string ScannerData
        {
            get => _ScannerData;
            set
            {
                if (_ScannerData != value)
                {
                    _ScannerData = value;
                    OnPropertyChanged(nameof(ScannerData));
                }
            }
        }

        

        private ActionCommand _SearchCommand;

        public ICommand SearchCommand
        {
            get
            {
                if (_SearchCommand == null)
                {
                    _SearchCommand = new ActionCommand(SearchData);
                }

                return _SearchCommand;
            }
        }

        private ActionCommand _ExportToExcelCommand;

        public ICommand ExportToExcelCommand
        {
            get
            {
                if (_ExportToExcelCommand == null)
                {
                    _ExportToExcelCommand = new ActionCommand(ExportToExcel);
                }

                return _ExportToExcelCommand;
            }
        }

        private TestResult _selectedResult;
        public TestResult SelectedResult
        {
            get => _selectedResult;
            set
            {
                _selectedResult = value;
                OnPropertyChanged(nameof(SelectedResult));
            }
        }

        private ICollectionView _resultsView;
        public ICollectionView ResultsView
        {
            get { return _resultsView; }
            set { _resultsView = value; OnPropertyChanged(nameof(ResultsView)); }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }

        private void SearchData()
        {
            ResultsView.Refresh(); // refresh lại filter
        }

        private bool FilterResults(object obj)
        {
            if (obj is TestResult result)
            {
                if (result.ERROR_CODE != AuToManualModeUtils.PASS_RESULT_CODE)
                    return false;

                if (string.IsNullOrEmpty(SearchText))
                    return true;

                return result.SerialNumber?.Contains(SearchText) == true
                       || result.QRCode?.Contains(SearchText) == true
                       || result.DateTime?.Contains(SearchText) == true;
            }
            return false;
        }


        private ObservableCollection<TestResult> _results = new ObservableCollection<TestResult>();
        public ObservableCollection<TestResult> Results
        {
            get => _results;
            set
            {
                _results = value;
                OnPropertyChanged(nameof(Results));
            }
        }

        private ObservableCollection<TestResult> _resultsDisplay = new ObservableCollection<TestResult>();
        public ObservableCollection<TestResult> ResultsDisplay
        {
            get => _resultsDisplay;
            set
            {
                _resultsDisplay = value;
                OnPropertyChanged(nameof(ResultsDisplay));
            }
        }



        public PrintResultDataGridViewModel( MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            var path = $"{_mainViewModel.application.pathStatistics}\\{_mainViewModel.application.defaultExcelFile}";
            System.IO.Directory.CreateDirectory(_mainViewModel.application.pathStatistics);
            var resultTemp = new List<TestResult>();
            ExcelHelper.LoadFromExcel(path, ref resultTemp);
            Results = new ObservableCollection<TestResult>(resultTemp);
            ResultsDisplay = new ObservableCollection<TestResult>(resultTemp.Where(s => s.ERROR_CODE == AuToManualModeUtils.PASS_RESULT_CODE));
            ResultsView = CollectionViewSource.GetDefaultView(ResultsDisplay);
            ResultsView.Filter = FilterResults;
            ResultsView.Refresh();
        }

        private void ExportToExcel()
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Excel Files (*.xlsx)|*.xlsx",
                FileName = "Results.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var visibleResults = ResultsView.Cast<TestResult>().ToList();
                ExcelHelper.SaveToExcel(saveFileDialog.FileName, visibleResults);
                MessageBox.Show("Export successfully!", "Message",
                                MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //public void UpdateResultToDataGrid()
        //{
        //    var result = AuToManualModeUtils.GetDataResult(ScanInput);
        //    Results.Add(result);
        //    ScanInput = string.Empty; // Clear input after scan
        //}
    }
}
