namespace DesktopProject.Components
{
    using System.Threading.Tasks;
    using DesktopProject.ViewModels;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using NeoIsisJob;
    using Workout.Core.IServices;

    public sealed partial class PostsFeed : UserControl
    {
        private const int PostsPerPage = 5;
        private int currentPage = 1;
        private PostViewModel postViewModel;

        public StackPanel PostsStackPanelPublic => this.PostsStackPanel;

        public PostsFeed()
        {
            this.InitializeComponent();

            var postService = App.Services.GetService<IPostService>();

            this.postViewModel = new PostViewModel(postService);
            this.Loaded += PostsFeed_Loaded;
        }

        private async void PostsFeed_Loaded(object sender, RoutedEventArgs e)
        {
            var postService = App.Services.GetService<IPostService>();
            this.postViewModel = new PostViewModel(postService);

            int userId = AppController.CurrentUser?.ID ?? -1;

            await this.postViewModel.PopulatePostsHomeFeed(userId);
            this.DisplayCurrentPage();
        }

        private async Task LoadPosts()
        {
            int userId;
            if (AppController.CurrentUser == null)
            {
                userId = -1;
            }
            else
            {
                userId = AppController.CurrentUser.ID;
            }

            await this.postViewModel.PopulatePostsHomeFeed(userId);
        }

        public void DisplayCurrentPage()
        {
            this.PostsStackPanel.Children.Clear();
            int startIndex = (currentPage - 1) * PostsPerPage;
            int endIndex = startIndex + PostsPerPage;
            var allPosts = this.postViewModel.GetCurrentPosts();
            for (int i = startIndex; i < endIndex && i < allPosts.Count; i++)
            {
                this.PostsStackPanel.Children.Add(allPosts[i]);
            }
        }

        public void ClearPosts()
        {
            this.postViewModel.ClearPosts();
        }

        public async Task PopulatePostsByGroupId(long groupId)
        {
            await this.postViewModel.PopulatePostsByGroupId(groupId);
        }

        public async Task PopulatePostsByUserId(int userId)
        {
            await this.postViewModel.PopulatePostsByUserId(userId);
        }

        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.currentPage > 1)
            {
                this.currentPage--;
                this.DisplayCurrentPage();
            }
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
            var allPosts = this.postViewModel.GetCurrentPosts();
            if (this.currentPage * PostsPerPage < allPosts.Count)
            {
                this.currentPage++;
                this.DisplayCurrentPage();
            }
        }
    }
}
