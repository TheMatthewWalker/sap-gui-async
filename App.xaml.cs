using costing_tool.Services;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using System.Diagnostics;
using Velopack;
using WinRT.Interop;
using System.IO;


namespace costing_tool
{

    public partial class App : Application
    {
        public static Window? MainWindow { get; private set; }
        public static MixingService MixingService { get; private set; }
        public static SapWorker SapWorker { get; private set; }
        public static AppConfig Config { get; private set; }
        public static string CurrentUser { get; set; } = string.Empty;
        public static bool IsLoggedIn { get; set; } = false;
        public static string CurrentPass { get; set; }  = string.Empty;


        public App()
        {
            InitializeComponent();
            this.UnhandledException += App_UnhandledException;

            // Initialize COM object once, on main STA thread
            SapWorker = new SapWorker();
            Config = AppConfig.Load();
            MixingService = new MixingService(Config.ApiBaseUrl, Config.ApiKey);
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            System.Diagnostics.Debug.WriteLine(e.Exception);
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            MainWindow = new MainWindow();

            IntPtr hwnd = WindowNative.GetWindowHandle(MainWindow);
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

            MainWindow.Activate();
        }
    }
}
