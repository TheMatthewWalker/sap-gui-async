using costing_tool.pages;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SAPFunctionsOCX;
using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

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

            int attempts = 0;
            const int maxAttempts = 3;

            var sap = new SapController();

            while (attempts < maxAttempts)
            {
                attempts++;

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

                        return;
                    }

                    sapMessage.Text = "Login failed. Please check your credentials.";
                    sapMessage.Visibility = Visibility.Visible;
                }
                catch (COMException comEx)
                {
                    sapMessage.Text =
                        $"SAP COM error on attempt {attempts}: {comEx.Message}";
                    sapMessage.Visibility = Visibility.Visible;
                    break;
                }
                catch (Exception ex)
                {
                    sapMessage.Text = $"Unexpected error: {ex.Message}";
                    sapMessage.Visibility = Visibility.Visible;
                    break;
                }

                await Task.Delay(300);
            }

            sapMessage.Text = "Unable to log on after multiple attempts.";
            sapMessage.Visibility = Visibility.Visible;
            LoginButton.IsEnabled = true;
        }

    }
}
