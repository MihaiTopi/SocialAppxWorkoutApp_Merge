namespace DesktopProject.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using DesktopProject.Components;
    using Workout.Core.Enums;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class PostViewModel
    {
        ObservableCollection<PostComponent> posts;
        private IPostService postService;

        public PostViewModel(IPostService postService)
        {
            this.postService = postService;
            this.posts = new ObservableCollection<PostComponent>();
        }

        public ObservableCollection<PostComponent> GetCurrentPosts()
        {
            return this.posts;
        }

        public void ClearPosts()
        {
            this.posts = new ObservableCollection<PostComponent>();
        }

        private void PopulatePosts(List<Post> receivedPosts)
        {
            foreach (Post post in receivedPosts)
            {
                var postComponent = new PostComponent(post.Title, post.Visibility, post.UserId, post.Content, post.CreatedDate, post.Tag, post.Id);
                this.posts.Add(postComponent);
            }
        }


        public async Task AddPost(string title, string? content, int userId, long? groupId, PostVisibility postVisibility, PostTag postTag)
        {
            await this.postService.AddPost(title, content, userId, groupId, postVisibility, postTag);
        }

        /// <summary>
        /// Deletes a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post to delete.</param>
        /*
        public void DeletePost(long id)
        {
            this.postService.DeletePost(id);
        }*/

        /// <summary>
        /// Updates a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post to update.</param>
        /// <param name="title">The new title of the post.</param>
        /// <param name="description">The new description of the post.</param>
        /// <param name="visibility">The new visibility of the post.</param>
        /// <param name="tag">The new tag of the post.</param>
        /*
        public void UpdatePost(long id, string title, string description, PostVisibility visibility, PostTag tag)
        {
            this.postService.UpdatePost(id, title, description, visibility, tag);
        }*/

        /// <summary>
        /// Gets all posts.
        /// </summary>
        /// <returns>A list of all posts.</returns>
        public async Task GetAllPosts()
        {
            var posts = await this.postService.GetAllPosts();
            this.PopulatePosts(posts);
        }

        /// <summary>
        /// Gets a post by ID.
        /// </summary>
        /// <param name="id">The ID of the post to retrieve.</param>
        /// <returns>The post with the specified ID.</returns>
        public async Task<Post> GetPostById(long id)
        {
            return await this.postService.GetPostById(id);
        }

        /// <summary>
        /// Gets posts by user ID.
        /// </summary>
        /// <param name="userId">The ID of the user whose posts to retrieve.</param>
        public async Task PopulatePostsByUserId(int userId)
        {
            var posts = await this.postService.GetPostsByUserId(userId);
        }

        public async Task PopulatePostsByGroupId(long groupId)
        {
            var posts = await this.postService.GetPostsByGroupId(groupId);
            this.PopulatePosts(posts);
        }

        //public void PopulatePostsGroupsFeed(long userId)
        //{
        //    var posts = this.postService.GetPostsGroupsFeed(userId);
        //}

        public async Task PopulatePostsHomeFeed(int userId)
        {
            var posts = await this.postService.GetPostsHomeFeed(userId);
            this.PopulatePosts(posts);
        }
    }
}
