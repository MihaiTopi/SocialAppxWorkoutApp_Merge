using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using NeoIsisJob;
using NeoIsisJob.Proxy;
using Workout.Core.Models;
using System.Collections.ObjectModel;

namespace DesktopProject.Pages
{
    public sealed partial class UserFollow : Page
    {
        private readonly UserServiceProxy userService;
        private ObservableCollection<DisplayUserModel> displayUsers = new();
        private HashSet<long> followingIds = new();

        public UserFollow()
        {
            this.InitializeComponent();

            userService = new UserServiceProxy();

            UserItemsControl.ItemsSource = displayUsers;

            LoadUsers();
        }

        private void LoadUsers(string search = "")
        {
            displayUsers.Clear();

            var currentUserId = AppController.CurrentUser.ID;

            var filteredUsers = userService.GetAllUsersAsync().Result
                .Where(u => u.ID != currentUserId &&
                            u.Username.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();

            followingIds = userService.GetUserFollowing(currentUserId)
                                      .Select(u => (long)u.ID)
                                      .ToHashSet();

            foreach (var user in filteredUsers)
            {
                bool isFollowing = followingIds.Contains(user.ID);
                displayUsers.Add(new DisplayUserModel
                {
                    ID = user.ID,
                    Username = user.Username,
                    FollowButtonText = isFollowing ? "Unfollow" : "Follow",
                    FollowButtonBackground = new SolidColorBrush(isFollowing ? Microsoft.UI.Colors.OrangeRed : Microsoft.UI.Colors.Green)
                });
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(HomeScreen));
        }

        public void FollowToggleButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            long targetId = (long)button.Tag;
            long currentUserId = AppController.CurrentUser.ID;

            if (followingIds.Contains(targetId))
            {
                userService.UnfollowUserById(currentUserId, targetId);
                followingIds.Remove(targetId);
            }
            else
            {
                userService.FollowUserById(currentUserId, targetId);
                followingIds.Add(targetId);
            }

            LoadUsers(SearchBox.Text);
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadUsers(SearchBox.Text);
        }

        public class DisplayUserModel : UserModel
        {
            public string FollowButtonText { get; set; }
            public SolidColorBrush FollowButtonBackground { get; set; }
        }
    }
}
