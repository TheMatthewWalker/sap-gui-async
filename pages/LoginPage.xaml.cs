using Avalonia.Controls;
using costing_tool.pages;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SAPFunctionsOCX;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

            sapMessage.Visibility = Visibility.Collapsed;
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
                        presenter.Maximize();

                        return;
                    }

                    sapMessage.Text = "Login failed. Please check your credentials.";
                    sapMessage.Visibility = Visibility.Visible;
                }
                catch (COMException comEx)
                {
                    sapMessage.Text =
                        $"SAP COM error : {comEx.Message}";
                    sapMessage.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    sapMessage.Text = $"Unexpected error: {ex.Message}";
                    sapMessage.Visibility = Visibility.Visible;
                }

            sapMessage.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = true;
        }

    }
}
