using costing_tool.pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SAPFunctionsOCX;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Velopack;
using WinRT.Interop;

namespace costing_tool.pages
{
    public sealed partial class LoginPage : Page
    {

        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Login_Click(object sender, RoutedEventArgs e)
        {
            string? user = sapUsername.Text;
            string? pass = sapPassword.Password;
            string? system = sapSystem.Text;

            sapMessage.IsOpen = false;
            LoginButton.IsEnabled = false;

            var sap = new SapController();

                try
                {
                    bool connected = await sap.LoginAsync(
                        system: system,
                        client: "100",
                        systemId: "01",
                        user: user,
                        password: pass
                    );

                    if (connected)
                    {

                        // Check for updates after login, non-blocking
                        _ = CheckForUpdatesAsync();

                        App.CurrentUser = user;
                        App.CurrentPass = pass;
                        App.IsLoggedIn = true;

                        if (this.Parent is Frame frame)
                            frame.Navigate(typeof(NavPage));

                        var m_Window = App.m_window;
                        IntPtr hwnd = WindowNative.GetWindowHandle(m_Window);
                        WindowId windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
                        AppWindow appWindow = AppWindow.GetFromWindowId(windowId);
                        var presenter = appWindow.Presenter as OverlappedPresenter;
                        appWindow.Resize(new Windows.Graphics.SizeInt32(1400, 1000));
                        presenter.Maximize();
                        presenter.IsMaximizable = true;

                    return;
                    }
                    else
                    {
                    ShowError("Invalid credentials. Please try again.");
                    LoginButton.IsEnabled = true;
                        return;
                    }

                }
                catch (InvalidOperationException ex)
                {
                // Specific handling for SapWorker init failures surfaced as InvalidOperationException
                ShowError("Please check SAP GUI is installed on your PC.");
                LoginButton.IsEnabled = true;

                    if (ex.InnerException != null)
                        Debug.WriteLine($"SAP initialization error (inner): {ex.InnerException}");
                    else
                        Debug.WriteLine($"SAP initialization error: {ex.Message}");
                }
                catch (COMException comEx)
                {
                ShowError(comEx.Message);
                LoginButton.IsEnabled = true;
                }
                catch (Exception ex)
                {
                ShowError(ex.Message);
                LoginButton.IsEnabled = true;
                }

            
        }

        private void ShowError(string message)
        {
            sapMessage.Message = message;
            sapMessage.IsOpen = true;
        }


        private async Task CheckForUpdatesAsync()
        {
            try
            {
                var updateManager = new UpdateManager(
                    new Velopack.Sources.SimpleFileSource(
                        new System.IO.DirectoryInfo(@"\\J:\Common\Logistics\_sap_gui")
                    )
                );

                var updateInfo = await updateManager.CheckForUpdatesAsync();
                if (updateInfo == null) return;

                var dialog = new ContentDialog
                {
                    Title = "Update Available",
                    Content = $"Version {updateInfo.TargetFullRelease.Version} is available. Update now?",
                    PrimaryButtonText = "Update Now",
                    SecondaryButtonText = "Later",
                    XamlRoot = this.XamlRoot
                };

                if (await dialog.ShowAsync() == ContentDialogResult.Primary)
                {
                    await updateManager.DownloadUpdatesAsync(updateInfo);
                    updateManager.ApplyUpdatesAndRestart(updateInfo);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Update check failed: {ex.Message}");
                // Silently fail - don't block the user if update server is unreachable
            }
        }

    }
}
