using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.ApplicationSettings;
using costing_tool.Models;

namespace costing_tool.pages;

public sealed partial class NavPage : Page
{
    public ObservableCollection<NavigationTiles> NavTiles { get; } = new();

    public NavPage()
    {
        InitializeComponent();
        LoadTiles();
    }

    private void LoadTiles()
    {
        NavTiles.Add(new NavigationTiles { Area = "Costing", IconPath = "ms-appx:///Assets/putaway.png" });
        NavTiles.Add(new NavigationTiles { Area = "Sales", IconPath = "ms-appx:///Assets/staging.png" });
        NavTiles.Add(new NavigationTiles { Area = "Production", IconPath = "ms-appx:///Assets/transfer.png" });
        NavTiles.Add(new NavigationTiles { Area = "Warehouse", IconPath = "ms-appx:///Assets/picklist.png" });
    }

    private void TileButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        if (btn.DataContext is not NavigationTiles tile) return;

        switch (tile.Area)
        {

            case "Costing":
                Frame.Navigate(typeof(CostPage));
                break;

            case "Production":
                Frame.Navigate(typeof(ProductionPage));
                break;

            default:
                // If needed: log unknown tile
                break;
        }
    }
}


