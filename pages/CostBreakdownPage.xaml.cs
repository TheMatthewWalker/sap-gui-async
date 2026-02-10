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

    public sealed partial class CostBreakdownPage : Page
    {

        public CostBreakdownPage()
        {
            this.InitializeComponent();

            // Bind DataContext for XAML
            this.DataContext = this;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!SapResultsState.HasResults)
            {
                
            }
            else
            {
                SapDataGrid.ItemsSource = SapResultsState.CostSheetResults;
            }
        }


        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}

