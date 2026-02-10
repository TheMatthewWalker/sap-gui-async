using CommunityToolkit.Common;
using CommunityToolkit.WinUI; // for AdvancedCollectionView
using costing_tool.pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SAPFunctionsOCX;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using static SapController;



namespace costing_tool.pages
{
    public class TableCOST
    {
        public string? Material { get; set; }
        public string? CostingDate { get; set; }
        public string? ProfitCenter { get; set; }
        public decimal DirectMaterial { get; set; }
        public decimal DirectLabour { get; set; }
        public decimal VariableProductionCost { get; set; }
        public decimal Scrap { get; set; }
        public decimal InboundFreight { get; set; }
        public decimal OutboundFreight { get; set; }
        public decimal Customs { get; set; }
        public decimal Packaging { get; set; }
        public decimal ExtrusionLabour { get; set; }
        public decimal Total { get; set; }
        public string? Unit { get; set; }
    }

    public class InitialCost
    {
        public string? Material { get; set; }
        public string? CostingDate { get; set; }
        public string? ProfitCenter { get; set; }
        public decimal Total { get; set; }
        public string? Unit { get; set; }
    }

    public class SearchCriteriaRow
    {
        public int RowNumber { get; set; } // For automatic numbering
        public string? Material { get; set; }
        public string? Volume { get; set; }
        public string? Incoterms { get; set; }
        public string? Country { get; set; }
    }

    public sealed partial class CostPage : Page
    {

        public ObservableCollection<SearchCriteriaRow> CriteriaRows { get; set; } = new();  // Define an ObservableCollection for search criteria rows

        public class SearchCriteriaRow : INotifyPropertyChanged
        {
            private int rowNumber;
            public int RowNumber
            {
                get => rowNumber;
                set
                {
                    if (rowNumber != value)
                    {
                        rowNumber = value;
                        OnPropertyChanged(nameof(RowNumber));
                    }
                }
            }

            private string material = string.Empty;
            public string Material
            {
                get => material;
                set
                {
                    if (material != value)
                    {
                        material = value;
                        OnPropertyChanged(nameof(Material));
                    }
                }
            }

            private string volume = string.Empty;
            public string Volume
            {
                get => volume;
                set
                {
                    if (volume != value)
                    {
                        volume = value;
                        OnPropertyChanged(nameof(Volume));
                    }
                }
            }

            private string incoterms = string.Empty;
            public string Incoterms
            {
                get => incoterms;
                set
                {
                    if (incoterms != value)
                    {
                        incoterms = value;
                        OnPropertyChanged(nameof(Incoterms));
                    }
                }
            }

            private string country = string.Empty;
            public string Country
            {
                get => country;
                set
                {
                    if (country != value)
                    {
                        country = value;
                        OnPropertyChanged(nameof(Country));
                    }
                }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            private void OnPropertyChanged(string propName) =>
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public CostPage()
        {
            this.InitializeComponent();

            // Add one initial row
            AddRow();

            // Bind DataContext for XAML
            this.DataContext = this;
        }

        private object? _contextRow;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SapResultsState.IsRunning == true)
            {
                CalcButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                StatusText.Text = "Reading SAP data...";
            }
            else
            { 
                if (!SapResultsState.HasResults)
                {
                    
                }
                else
                {
                    SapDataGrid.ItemsSource = SapResultsState.InitialCostResults;
                }
            }
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            AddRow();
        }

        private void AddRow()
        {
            int nextRow = CriteriaRows.Count + 1;
            CriteriaRows.Add(new SearchCriteriaRow { RowNumber = nextRow });
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is SearchCriteriaRow row)
            {
                CriteriaRows.Remove(row);

                // Re-number remaining rows
                for (int i = 0; i < CriteriaRows.Count; i++)
                {
                    CriteriaRows[i].RowNumber = i + 1; // triggers PropertyChanged
                }
            }
        }

        private string[][] BuildMaterialArray()
        {
            var materialFilters = new List<string[]>();

            foreach (var row in CriteriaRows)
            {
                // Example: only include rows that have a value
                if (!string.IsNullOrWhiteSpace(row.Material))
                    materialFilters.Add(new[] { row.Material, row.Volume ?? "", row.Incoterms ?? "", row.Country ?? "" });
            }

            return materialFilters.ToArray();
        }

        private async void Read_Table_Click(object sender, RoutedEventArgs e)
        {

            var stopwatch = Stopwatch.StartNew();
            CalcButton.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
            StatusText.Text = "Reading SAP data...";
            SapResultsState.IsRunning = true;

            SapController sap = new SapController();

            try
            {
                var materialFilters = BuildMaterialArray();
                var rows = await sap.CostSheetAsync(materialFilters);
                stopwatch.Stop();

                var initialRows = rows.QuickData
                    .OrderBy(r => r.Material)
                    .ToList();

                var allRows = rows.AllData
                    .OrderBy(r => r.Material)
                    .ToList();

                // STORE RESULTS
                SapResultsState.CostSheetResults = allRows;
                SapResultsState.InitialCostResults = initialRows;
                SapResultsState.LastMaterialFilters = materialFilters;
                SapResultsState.IsRunning = false;

                SapDataGrid.ItemsSource = initialRows;
                SapDataGrid.CanUserSortColumns = true;

                StatusText.Text =
                    $"Loaded {initialRows.Count} rows in {stopwatch.Elapsed.TotalSeconds:F1}s";
            }
            catch (Exception ex)
            {
                StatusText.Text = ex.Message;
            }
            finally
            {
                CalcButton.IsEnabled = true;
                ProgressBar.Visibility = Visibility.Collapsed;
            }
        }


        private void SapDataGrid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            // Determine which row was clicked
            var row = (e.OriginalSource as FrameworkElement)?.DataContext;

            if (row != null)
                _contextRow = row;   // store the row for the context menu actions
        }

        private void ViewCost_Click(object sender, RoutedEventArgs e)
        {
            if (_contextRow is InitialCost row)
            {
                // Navigate to your cost breakdown page
                Frame.Navigate(typeof(CostBreakdownPage), row);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

