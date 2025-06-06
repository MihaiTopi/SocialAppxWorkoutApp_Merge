namespace DesktopProject.Components
{
    using DesktopProject;
    using DesktopProject.Pages;
    using DesktopProject.Windows;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using NeoIsisJob.Views;

    public sealed partial class TopBar : UserControl
    {
        private Frame frame;

        public TopBar()
        {
            this.InitializeComponent();
        }

        public void SetFrame(Frame frame)
        {
            this.frame = frame;
            SetPhoto();
            SetNavigationButtons();
        }

        private async void SetPhoto()
        {
            if (AppController.CurrentUser != null && !string.IsNullOrEmpty(string.Empty))
            {
                //UserImage.Source = await AppController.DecodeBase64ToImageAsync(controller.CurrentUser.Image);
            }
        }

        private void SetNavigationButtons()
        {

            //SeeUsersButton.Click += UserClick;
            GroupsButton.Click += GroupsClick;
            CreatePostButton.Click += CreatePostButton_Click;
            SeeUsersButton.Click += SeeUsersClick;
            
        }

        private void GoToNeo(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(MainPage));
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            frame.Navigate(typeof(HomeScreen));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {

            
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            frame.Navigate(typeof(GroupsScreen));

        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(LoginPage));
        }

        private void SeeUsersClick(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(UserFollow));
        }

        private void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(CreatePost));
        }


        public void SetHome()
        {
            
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        }
        public void SetGroups()
        {
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        }
        public void SetCreate()
        {
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Blue);
        }
        public void SetNone()
        {
            GroupsButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
            CreatePostButton.Foreground = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        }

        public Button GroupsButtonInstance => GroupsButton;
        public Button CreatePostButtonInstance => CreatePostButton;
        public Button UserButtonInstance => SeeUsersButton;
    }
}
