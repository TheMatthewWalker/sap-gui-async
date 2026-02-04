using CommunityToolkit.Common;
using CommunityToolkit.WinUI; // for AdvancedCollectionView
using costing_tool.pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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



namespace costing_tool.pages
{
    public class TableCOST
    {
        public string? Material { get; set; }
        //public string WERKS { get; set; }
        public string? CostingDate { get; set; }
        //public string BIDAT { get; set; }
        public string? ProfitCenter { get; set; }
        //public string BUKRS { get; set; }
        //public string PATNR { get; set; }
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
        //public decimal Depreciation { get; set; }
        //public decimal Tariffs { get; set; }
        //public decimal PricePer { get; set; }
        public string? Unit { get; set; }
        //public string FEH_STA { get; set; }
        //public string WERK { get; set; }
        //public string VALID_FROM { get; set; }
        //public string VALID_TO { get; set; }
        //public string OH_PCT { get; set; }
        //public string IC_MARK_UP { get; set; }

    }

    public class SearchCriteriaRow
    {
        public int RowNumber { get; set; } // For automatic numbering
        public string? Material { get; set; }
        public string? Volume { get; set; }
        public string? Incoterms { get; set; }
        public string? Country { get; set; }
    }

    public sealed partial class OutputPage : Page
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

        public OutputPage()
        {
            this.InitializeComponent();

            // Add one initial row
            AddRow();

            // Bind DataContext for XAML
            this.DataContext = this;
        }

        private object? _contextRow;
        private CancellationTokenSource? _sapCts;

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

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (_sapCts != null)
            {
                _sapCts.Cancel();
                StatusText.Text = "Cancelling SAP query...";
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


            // Disable UI
            var stopwatch = Stopwatch.StartNew();
            CalcButton.IsEnabled = false;
            //CancelButton.Visibility = Visibility.Visible;
            ProgressBar.Visibility = Visibility.Visible;
            StatusText.Text = "Reading SAP data...";
            //_sapCts = new CancellationTokenSource();

            SapController sap = new SapController();


            try
            {
                var materialFilters = BuildMaterialArray();
                var rows = await sap.CostSheetAsync(/*_sapCts.Token,*/ materialFilters);
                stopwatch.Stop();
                var sortedRows = rows.OrderBy(r => r.Material).ToList();

                SapDataGrid.ItemsSource = sortedRows;
                SapDataGrid.CanUserSortColumns = true;
                StatusText.Text = $"Loaded {rows.Count} rows in {stopwatch.Elapsed.TotalSeconds:F1}s";

            }
            catch (TaskCanceledException)
            {
                StatusText.Text = "SAP query cancelled.";
            }
            catch (COMException ex)
            {
                StatusText.Text = $"SAP COM error (0x{ex.HResult:X}): {ex.Message}";
            }
            catch (Exception ex)
            {
                StatusText.Text = $"C# error ({ex.Message}";
            }
            finally
            {
                CalcButton.IsEnabled = true;
                ProgressBar.Visibility = Visibility.Collapsed;
                //CancelButton.Visibility = Visibility.Collapsed;
                //_sapCts = null;
            }
        }
        
        private void SapDataGrid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            // Determine which row was clicked
            var row = (e.OriginalSource as FrameworkElement)?.DataContext;

            if (row != null)
                _contextRow = row;   // store the row for the context menu actions
        }

        private void CreateMovement_Click(object sender, RoutedEventArgs e)
        {
            if (_contextRow is TableCOST row)
            {
                // Navigate to your warehouse movement page
                Frame.Navigate(typeof(OutputPage), row);
            }
        }

        private void ViewCost_Click(object sender, RoutedEventArgs e)
        {
            if (_contextRow is TableCOST row)
            {
                StatusText.Text = $"Viewing cost breakdown for {row.Material}";
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

