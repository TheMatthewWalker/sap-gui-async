using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Windows.UI.ApplicationSettings;
using costing_tool.Models;

namespace costing_tool.pages;

public sealed partial class ProductionPage : Page
{
    public ObservableCollection<ProductionTiles> ProdTiles { get; } = new();

    public ProductionPage()
    {
        InitializeComponent();
        LoadTiles();
    }

    private void LoadTiles()
    {
        ProdTiles.Add(new ProductionTiles { Area = "Mixing", IconPath = "ms-appx:///Assets/putaway.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Extrusion & Tower", IconPath = "ms-appx:///Assets/staging.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Convoluting", IconPath = "ms-appx:///Assets/transfer.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Tape Wrap", IconPath = "ms-appx:///Assets/picklist.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Braiding", IconPath = "ms-appx:///Assets/picklist.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Coverline", IconPath = "ms-appx:///Assets/picklist.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Ewald", IconPath = "ms-appx:///Assets/picklist.png" });
        ProdTiles.Add(new ProductionTiles { Area = "Drumming", IconPath = "ms-appx:///Assets/picklist.png" });
    }

    private void TileButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not Button btn) return;
        if (btn.DataContext is not ProductionTiles tile) return;

        switch (tile.Area)
        {

            case "Mixing":
                Frame.Navigate(typeof(MixingPage));
                break;


            default:
                // If needed: log unknown tile
                break;
        }
    }
}


