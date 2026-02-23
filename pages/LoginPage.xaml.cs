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
using System.Diagnostics;

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

    }
}
