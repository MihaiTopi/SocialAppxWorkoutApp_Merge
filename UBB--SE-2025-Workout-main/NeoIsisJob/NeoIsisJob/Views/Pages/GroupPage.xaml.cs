using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopProject.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using NeoIsisJob;
using Workout.Core.IServices;
using Workout.Core.Models;


namespace DesktopProject.Pages
{
    public sealed partial class GroupPage : Page
    {
        private const Visibility collapsed = Visibility.Collapsed;
        private const Visibility visible = Visibility.Visible;
        private IUserService userService;
        private IPostService postService;
        private IGroupService groupService;
        private long GroupId;
        private Group group;

        public GroupPage()
        {
            this.InitializeComponent();
            this.Loaded += this.DisplayPageAsync; // Fix: Use a method group that matches the RoutedEventHandler delegate
        }

        private async void DisplayPageAsync(object sender, RoutedEventArgs e) // Fix: Change the method signature to match RoutedEventHandler
        {
            await this.DisplayPage(sender, e);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is long id)
            {
                this.GroupId = id;
            }

            this.TopBar.SetFrame(this.Frame);
            this.TopBar.SetNone();
        }

        private async Task DisplayPage(object sender, RoutedEventArgs e)
        {
            this.userService = App.Services.GetService<IUserService>();
            this.groupService = App.Services.GetService<IGroupService>();
            this.postService = App.Services.GetService<IPostService>();

            this.group = await this.groupService.GetGroupById(this.GroupId);

            await this.SetContent();
            await this.PopulateMembers();
        }

        private async Task SetContent()
        {
            GroupTitle.Text = group.Name;
            GroupDescription.Text = group.Description;
            // if (!string.IsNullOrEmpty(group.Image))
            // GroupImage.Source = await AppController.DecodeBase64ToImageAsync(group.Image);
            await this.PopulateFeed();
        }

        private async Task PopulateFeed()
        {
            this.PostsFeed.ClearPosts();
            await this.PostsFeed.PopulatePostsByGroupId(GroupId);
            PostsFeed.Visibility = Visibility.Visible;
            PostsFeed.DisplayCurrentPage();
        }

        private async Task PopulateMembers()
        {
            this.MembersList.Children.Clear();
            List<UserModel> members = await groupService.GetUsersFromGroup(GroupId);
            foreach (UserModel member in members)
            {
                this.MembersList.Children.Add(new Member(member, this.Frame, GroupId));
            }
        }

        private void CreatePostInGroupButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreatePost));
        }
    }
}