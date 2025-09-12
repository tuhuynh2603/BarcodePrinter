using BarcodePrintLabel.Core;
using BarcodePrintLabel.Core.Commands;
using BarcodePrintLabel.Core.Services;
using BarcodePrintLabel.Database;
using BarcodePrintLabel.Database.Repositories;
using BarcodePrintLabel.Models;
using BarcodePrintLabel.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Xaml.Behaviors.Core;
using MySqlX.XDevAPI.Common;
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
                if (result.RESULT != AuToManualModeUtils.PASS_RESULT_CODE)
                    return false;

                if (string.IsNullOrEmpty(SearchText))
                    return true;

                return result.SerialNumber?.Contains(SearchText) == true
                       || result.QRCode?.Contains(SearchText) == true
                       || result.DateTime.ToString().Contains(SearchText) == true;
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

        private readonly DatabaseContext _db;
        ITestResultRepository repository ;
        private readonly TestResultService _service;


        public PrintResultDataGridViewModel( MainViewModel mainViewModel )
        {
            _mainViewModel = mainViewModel;
            var path = $"{_mainViewModel.application.pathStatistics}\\{_mainViewModel.application.defaultExcelFile}";
            System.IO.Directory.CreateDirectory(_mainViewModel.application.pathStatistics);
            //var resultTemp = new List<TestResult>();

            //ExcelHelper.LoadFromExcel(path, ref resultTemp);

            _db = new DatabaseContext(_mainViewModel.application.sqlConnectorString);
            _db.Database.EnsureCreated();
            repository = new TestResultRepository(_db);
            _service = new TestResultService(repository);

            LoadData();
        }

        private void LoadData()
        {
            var data = _db.TestResults.OrderBy(r => r.DateTime).ToList();
            DateTime oldestDate = DateTime.Now.AddDays(-_mainViewModel.application.numberDayKeepData);
            Results = new ObservableCollection<TestResult>(data.Where(r => r.DateTime >= oldestDate).OrderBy(r => r.DateTime));
            ResultsDisplay = new ObservableCollection<TestResult>(Results.Where(s => s.RESULT == AuToManualModeUtils.PASS_RESULT_CODE));
            ResultsView = CollectionViewSource.GetDefaultView(ResultsDisplay);
            ResultsView.Filter = FilterResults;
            ResultsView.Refresh();

        }

        public async Task LoadDataAsync()
        {
            Results.Clear();
            var results = await _service.GetResultsAsync();
            foreach (var result in results)
            {
                Results.Append(result);
            }

            DateTime oldestDate = DateTime.Now.AddDays(-30);
            Results = new ObservableCollection<TestResult>(Results.Where(r => r.DateTime >= oldestDate).OrderBy(r => r.DateTime));

            ResultsDisplay = new ObservableCollection<TestResult>(Results.Where(s => s.RESULT == AuToManualModeUtils.PASS_RESULT_CODE));
            ResultsView = CollectionViewSource.GetDefaultView(ResultsDisplay);
            ResultsView.Filter = FilterResults;
            ResultsView.Refresh();
        }

        public async Task AddDataToSQL( TestResult result )
        {

            await _service.AddResultAsync(result);
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
