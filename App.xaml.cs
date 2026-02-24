using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using costing_tool.Services;
using System;
using Velopack;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace costing_tool
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    /// 


    public partial class App : Application
    {
        public static Window? m_window { get; private set; }
        public static MixingService MixingService { get; private set; }
        public static SapWorker SapWorker { get; private set; }
        public static string CurrentUser { get; set; } = string.Empty;
        public static bool IsLoggedIn { get; set; } = false;
        public static string CurrentPass { get; set; }  = string.Empty;


        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            InitializeComponent();
            this.UnhandledException += App_UnhandledException;

            // Initialize COM object once, on main STA thread
            //SapFuncs = new SAPFunctions();
            //Moved from UI thread to own seperate thread in SapController.cs
            SapWorker = new SapWorker();
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            System.Diagnostics.Debug.WriteLine(e.Exception);
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();

            var config = AppConfig.Load();
            MixingService = new MixingService(config.ApiBaseUrl, config.ApiKey);

            IntPtr hwnd = WindowNative.GetWindowHandle(m_window);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            if (appWindow.Presenter is OverlappedPresenter overlapped)
            {
                overlapped.IsMaximizable = false;
                overlapped.IsMinimizable = false;
                overlapped.IsResizable = false;
                appWindow.Resize(new Windows.Graphics.SizeInt32(800, 900));
                appWindow.TitleBar.ExtendsContentIntoTitleBar = true;
                //overlapped.SetBorderAndTitleBar(false, false); // Removes border and titlebar completely

                // Background colours
                //appWindow.TitleBar.ButtonBackgroundColor = Colors.Transparent;
                //appWindow.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                //appWindow.TitleBar.ButtonHoverBackgroundColor = Colors.Transparent;
                //appWindow.TitleBar.ButtonPressedBackgroundColor = Colors.Transparent;

                // Foreground (the actual icons) - set to transparent to hide them
                appWindow.TitleBar.ButtonForegroundColor = Colors.Black;
                //appWindow.TitleBar.ButtonInactiveForegroundColor = Colors.Transparent;
                //appWindow.TitleBar.ButtonHoverForegroundColor = Colors.Transparent;
                //appWindow.TitleBar.ButtonPressedForegroundColor = Colors.Transparent;
            }

            m_window.Activate();
        }
    }
}
