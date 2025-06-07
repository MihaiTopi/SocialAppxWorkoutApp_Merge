namespace DesktopProject.Components
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DesktopProject.Proxies;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using NeoIsisJob.Proxy;
    using NeoIsisJob.Views;
    using ServerLibraryProject.Interfaces;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    /// <summary>
    /// Represents a user control for displaying a follower in the social app.
    /// </summary>
    public sealed partial class Follower : UserControl
    {
        private readonly UserModel user;

        private readonly Frame navigationFrame;

        //private IUserService userServiceProxy;
        private UserServiceProxy userServiceProxy;

        private IPostRepository postRepository;

        private IPostService postService;

        private IGroupService groupServiceProxy;

        /// <summary>
        /// Initializes a new instance of the <see cref="Follower"/> class.
        /// <summary>
        /// Initializes a new instance of the <see cref="Follower"/> class.
        /// </summary>
        /// <param name="username">The username of the follower.</param>
        /// <param name="isFollowing">Indicates whether the current user is following this user.</param>
        /// <param name="user">The user object associated with the follower.</param>
        /// <param name="frame">The navigation frame for navigating to user pages.</param>
        public Follower(string username, bool isFollowing, UserModel user, Frame frame = null)
        {
            this.InitializeComponent();

            this.userServiceProxy = new UserServiceProxy();
            this.groupServiceProxy = new GroupServiceProxy(); //ar trebui sa fie Service si cu dependency injection nu cu new trebuie schimbat
            this.postService = App.Services.GetService<IPostService>();

            this.user = user;
            this.navigationFrame = frame ?? Window.Current.Content as Frame; // Fallback to app-level Frame if not provided
            this.Name.Text = username;

            // Update the button content asynchronously after initialization
            this.InitializeButtonContentAsync();
        }

        /// <summary>
        /// Asynchronously initializes the button content based on the follow status.
        /// </summary>
        private async void InitializeButtonContentAsync()
        {
            this.Button.Content = await this.IsFollowed() ? "Unfollow" : "Follow";
        }

        /// <summary>
        /// Determines whether the current user is following this user.
        /// </summary>
        /// <returns>True if the user is followed; otherwise, false.</returns>
        private async Task<bool> IsFollowed()
        {
            List<UserModel> following = await this.userServiceProxy.GetUserFollowing(AppController.CurrentUser.ID);
            foreach (UserModel user in following)
            {
                if (user.ID == this.user.ID)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Handles the click event for the follower control.
        /// </summary>
        private void Follower_Click(object sender, RoutedEventArgs e)
        {
            if (this.navigationFrame != null)
            {
                this.navigationFrame.Navigate(typeof(LoginPage), new UserPageNavigationArgs(this.user));
            }
        }

        /// <summary>
        /// Handles the click event for the follow/unfollow button.
        /// </summary>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Button.Content = this.Button.Content.ToString() == "Follow" ? "Unfollow" : "Follow";
            if (await this.IsFollowed())
            {
                await this.userServiceProxy.FollowUserById(AppController.CurrentUser.ID, this.user.ID);
            }
            else
            {
                await this.userServiceProxy.UnfollowUserById(AppController.CurrentUser.ID, user.ID);
            }
        }
    }

    /// <summary>
    /// Helper class to pass both controller and user for navigation.
    /// </summary>
    public class UserPageNavigationArgs
    {
        /// <summary>
        /// Gets the application controller.
        /// </summary>

        /// <summary>
        /// Gets the selected user.
        /// </summary>
        public UserModel SelectedUser { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserPageNavigationArgs"/> class.
        /// </summary>
        /// <param name="controller">The application controller.</param>
        /// <param name="selectedUser">The selected user.</param>
        public UserPageNavigationArgs(UserModel selectedUser)
        {
            SelectedUser = selectedUser;
        }
    }
}