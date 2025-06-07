namespace DesktopProject.Windows
{
    using DesktopProject.Pages;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using Microsoft.UI.Xaml.Navigation;

    public sealed partial class GroupsScreen : Page
    {
        public GroupsScreen()
        {
            this.InitializeComponent();
            GroupsDrawer.NavigationFrame = this.Frame;
            this.SetNavigation();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            TopBar.SetFrame(this.Frame);
            TopBar.SetGroups();
            GroupsDrawer.NavigationFrame = this.Frame;
        }

        private void SetNavigation()
        {
            TopBar.HomeButtonInstance.Click += HomeClick; // Updated to use the public property  
            TopBar.UsersButtonInstance.Click += UserClick;
            TopBar.GroupsButtonInstance.Click += GroupsClick;
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeScreen));
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(UserFollow));
        }

        private void GroupsClick(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(GroupsScreen));
        }
    }
}