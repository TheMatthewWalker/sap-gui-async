using Microsoft.UI.Xaml;
using costing_tool.pages;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace costing_tool
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {

        public MainWindow()
        {
            this.InitializeComponent();

            // Set initial page in frame
            MainFrame.Navigate(typeof(LoginPage));
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Exit();
        }

        private void MinimiseButton_Click(object sender, RoutedEventArgs e)
        {
            var presenter = AppWindow.Presenter as OverlappedPresenter;
            presenter?.Minimize();
        }
    }
}
