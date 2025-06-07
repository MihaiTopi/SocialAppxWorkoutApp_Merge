namespace NeoIsisJob.Proxy
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using ServerLibraryProject.Models;
    using Workout.Core.IServices;

    public class CommentServiceProxy : ICommentService
    {
        private readonly HttpClient httpClient;

        public CommentServiceProxy()
        {

            httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:5261/api/comments/"),
            };
        }

        /// <summary>
        /// Adds a new comment.
        /// </summary>
        public async Task<Comment> AddComment(string content, int userId, long postId)
        {
            var comment = new Comment
            {
                Content = content,
                UserId = userId,
                PostId = postId,

                CreatedDate = DateTime.UtcNow,
            };

            var response = await httpClient.PostAsJsonAsync(string.Empty, comment);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Comment>();
            }
            throw new Exception($"Failed to add comment: {response.StatusCode}");
        }

        /// <summary>
        /// Deletes a comment by its ID.
        /// </summary>
        //public void DeleteComment(long commentId)
        //{

        //    var response = this._httpClient.DeleteAsync($"{commentId}").Result;
        //    response.EnsureSuccessStatusCode();
        //}

        /// <summary>
        /// Retrieves all comments.
        /// </summary>
        public async Task<List<Comment>> GetAllComments()
        {
            var response = await this.httpClient.GetAsync(string.Empty);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Comment>>();
            }
            throw new Exception($"Failed to get comments: {response.StatusCode}");
        }

        /// <summary>
        /// Retrieves a comment by its ID.
        /// </summary>
        //public Comment GetCommentById(int commentId)
        //{

        //    return this._httpClient.GetFromJsonAsync<Comment>($"{commentId}").Result!;
        //}

        /// <summary>
        /// Retrieves all comments for a specific post.
        /// </summary>
        public async Task<List<Comment>> GetCommentsByPostId(long postId)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5261/api/posts/{postId}/comments");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Comment>>();
            }
            throw new Exception($"Failed to get comments: {response.StatusCode}");

        }

        /// <summary>
        /// Updates the content of an existing comment.
        /// </summary>
        //public void UpdateComment(long commentId, string content)
        //{
        //    var commentDto = new
        //    {

        //        Content = content,
        //    };

        //    var response = this._httpClient.PutAsJsonAsync($"{commentId}", commentDto).Result;
        //    response.EnsureSuccessStatusCode();
        //}
    }
}
