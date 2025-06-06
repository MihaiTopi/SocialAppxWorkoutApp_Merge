using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using NeoIsisJob.Proxy;
using Workout.Core.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace NeoIsisJob.Views
{
    public sealed partial class LoginPage : Page
    {
        private UserServiceProxy userService;
        public LoginPage()
        {
            this.InitializeComponent();
            this.userService = new UserServiceProxy();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text.Trim();
            string password = PasswordBox.Password;

            UserModel? user = this.userService.GetUserByUsername(username);
            if (user == null)
            {
                // Empty fields
                ErrorMessageTextBlock.Text = "User doesn't exist";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
            else
            {
                AppController.CurrentUser = user;
                // Clear the form
                UsernameTextBox.Text = "";
                PasswordBox.Password = "";
                SuccessMessageTextBlock.Text = $"Welcome, {username}! Redirecting to main page...";
                SuccessMessageTextBlock.Visibility = Visibility.Visible;
                if (MainWindow.AppMainFrame != null)
                {
                    MainWindow.AppMainFrame.Navigate(typeof(MainPage));
                }
            }
        }
    }
}