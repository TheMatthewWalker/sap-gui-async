using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.ApplicationSettings;

namespace costing_tool.pages;

public sealed partial class NavPage : Page
{
    public ObservableCollection<TileItem> TileItems { get; } = new();

    public NavPage()
    {
        InitializeComponent();
        LoadTiles();
    }

    private void LoadTiles()
    {
        TileItems.Add(new TileItem { Title = "Costing", IconPath = "ms-appx:///Assets/putaway.png" });
        TileItems.Add(new TileItem { Title = "Sales", IconPath = "ms-appx:///Assets/staging.png" });
        TileItems.Add(new TileItem { Title = "Production", IconPath = "ms-appx:///Assets/transfer.png" });
        TileItems.Add(new TileItem { Title = "Warehouse", IconPath = "ms-appx:///Assets/picklist.png" });
    }

    private void TileButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        if (btn.DataContext is not TileItem tile) return;

        switch (tile.Title)
        {

            case "Costing":
                Frame.Navigate(typeof(CostPage));
                break;

            default:
                // If needed: log unknown tile
                break;
        }
    }
}

public class TileItem
{
    public string Title { get; set; } = string.Empty;
    public string IconPath { get; set; } = string.Empty;
}
