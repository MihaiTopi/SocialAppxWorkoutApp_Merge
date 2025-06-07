using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using NeoIsisJob;
using NeoIsisJob.Proxy;
using Workout.Core.Models;

namespace DesktopProject.Pages
{
    public sealed partial class UserFollow : Page
    {
        //private readonly IUserService userService;
        private readonly UserServiceProxy userService;
        private List<UserModel> allUsers = new();
        private HashSet<int> followingIds = new();

        public UserFollow()
        {
            this.InitializeComponent();

            //userService = App.Services.GetService<IUserService>();
            userService = new UserServiceProxy();

            this.Loaded += UserFollow_Loaded;
        }

        private async void UserFollow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadUsers();
        }

        private async Task LoadUsers(string search = "")
        {
            UserListPanel.Children.Clear();

            var currentUserId = AppController.CurrentUser.ID;

            // All other users except current
            // Inside an async method:
            allUsers = (await userService.GetAllUsersAsync())
                .Where(u => u.ID != currentUserId &&
                            u.Username.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();


            followingIds = (await userService.GetUserFollowing(currentUserId))
                                      .Select(u => u.ID)
                                      .ToHashSet();

            foreach (var user in allUsers)
            {
                var rowGrid = new Grid
                {
                    Margin = new Thickness(0, 0, 0, 10)
                };
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                rowGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100) });

                var nameText = new TextBlock
                {
                    Text = user.Username,
                    FontSize = 16,
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.Black),
                    VerticalAlignment = VerticalAlignment.Center
                };
                Grid.SetColumn(nameText, 0);
                rowGrid.Children.Add(nameText);

                var button = new Button
                {
                    Content = followingIds.Contains(user.ID) ? "Unfollow" : "Follow",
                    Background = new SolidColorBrush(followingIds.Contains(user.ID)
                        ? Microsoft.UI.Colors.OrangeRed
                        : Microsoft.UI.Colors.Green),
                    Foreground = new SolidColorBrush(Microsoft.UI.Colors.White),
                    Width = 80,
                    Height = 30,
                    Tag = user.ID
                };

                // Remove hover flicker by setting style directly
                button.Resources["ButtonBackgroundPointerOver"] = button.Background;
                button.Resources["ButtonForegroundPointerOver"] = button.Foreground;

                button.Click += ToggleFollow_Click;
                Grid.SetColumn(button, 1);
                rowGrid.Children.Add(button);

                UserListPanel.Children.Add(rowGrid);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Change this to the page you want to navigate to
            this.Frame.Navigate(typeof(HomeScreen));
        }

        private async void ToggleFollow_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            int targetId = (int)button.Tag;
            int currentUserId = AppController.CurrentUser.ID;

            if (followingIds.Contains(targetId))
            {
                await userService.UnfollowUserById(currentUserId, targetId);
            }
            else
            {
                await userService.FollowUserById(currentUserId, targetId);
            }

            await LoadUsers(SearchBox.Text); // Refresh UI
        }

        private async void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            await LoadUsers(SearchBox.Text);
        }
    }
}