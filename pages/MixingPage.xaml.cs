using costing_tool.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace costing_tool.pages
{
    public sealed partial class MixingPage : Page
    {
        public ObservableCollection<MixingCriteria> MixingCriteria { get; set; } = new();  // Define an ObservableCollection for search criteria

        // Hardcoded shift options
        public List<string> ShiftOptions { get; }

        public MixingPage()
        {
            this.InitializeComponent();

            ShiftOptions = App.Config.ShiftOptions;

            // Add one initial row
            AddRow();

            // Bind DataContext for XAML
            this.DataContext = this;
        }

        private object? _contextRow;


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (SqlMixingResults.IsRunning == true)
            {
                QueryButton.IsEnabled = false;
                ProgressBar.Visibility = Visibility.Visible;
                StatusText.Text = "Reading SQL data...";
            }
            else
            {
                if (!SqlMixingResults.HasResults)
                {

                }
                else
                {
                    SqlDataGrid.ItemsSource = SqlMixingResults.MixingResults;
                }
            }
        }

        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            AddRow();
        }

        private void AddRow()
        {
            int nextRow = MixingCriteria.Count + 1;
            MixingCriteria.Add(new MixingCriteria { RowNumber = nextRow });
        }

        private void DeleteRowButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is MixingCriteria row)
            {
                MixingCriteria.Remove(row);
            }
        }


        private async void Sql_Query_Click(object sender, RoutedEventArgs e)
        {

            var stopwatch = Stopwatch.StartNew();
            QueryButton.IsEnabled = false;
            ProgressBar.Visibility = Visibility.Visible;
            StatusText.Text = "Reading SQL data...";
            SqlMixingResults.IsRunning = true;

            try
            {
                var rows = await App.MixingService.SearchAsync(MixingCriteria);
                stopwatch.Stop();

                SqlMixingResults.MixingResults = rows;
                SqlMixingResults.IsRunning = false;

                SqlDataGrid.ItemsSource = rows;
                SqlDataGrid.CanUserSortColumns = true;

                StatusText.Text =
                    $"Loaded {rows.Count} rows in {stopwatch.Elapsed.TotalSeconds:F1}s";
            }
            catch (Exception ex)
            {
                StatusText.Text = ex.Message;
            }
            finally
            {
                QueryButton.IsEnabled = true;
                ProgressBar.Visibility = Visibility.Collapsed;
                SqlMixingResults.IsRunning = false;
            }
        }


        private void SqlDataGrid_RightTapped(object sender, Microsoft.UI.Xaml.Input.RightTappedRoutedEventArgs e)
        {
            // Determine which row was clicked
            var row = (e.OriginalSource as FrameworkElement)?.DataContext;

            if (row != null)
                _contextRow = row;   // store the row for the context menu actions
        }

        private void Reprint_Click(object sender, RoutedEventArgs e)
        {
            if (_contextRow is Mixing row)
            {
                // Print paperwork
            }
        }

        private void View_Details_Click(object sender, RoutedEventArgs e)
        {
            if (_contextRow is Mixing row)
            {
                // Navigate to your mixing breakdown page
                //Frame.Navigate(typeof(MixingViewPage), row);
            }
        }

        private void Edit_Details_Click(object sender, RoutedEventArgs e)
        {
            if (_contextRow is Mixing row)
            {
                // Navigate to your mixing breakdown page
                //Frame.Navigate(typeof(MixingEditPage), row);
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
