namespace ServerAPIProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Models;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly IPostService postService;
        private readonly IReactionService reactionService;
        private readonly ICommentService commentService;

        public PostController(IPostService postService, IReactionService reactionService, ICommentService commentService)
        {
            this.postService = postService;
            this.reactionService = reactionService;
            this.commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetAllPosts()
        {
            try
            {
                return this.Ok(await this.postService.GetAllPosts());
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving posts: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPostById(long id)
        {
            try
            {
                var post = await this.postService.GetPostById(id);
                if (post == null)
                {
                    return this.NotFound($"Post with ID {id} not found.");
                }

                return this.Ok(post);
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving post: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<List<Post>>> GetPostsByUserId(int userId)
        {
            try
            {
                return this.Ok(await this.postService.GetPostsByUserId(userId));
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving user's posts: {ex.Message}");
            }
        }

        [HttpGet("group/{groupId}")]
        public async Task<ActionResult<List<Post>>> GetPostsByGroupId(long groupId)
        {
            try
            {
                return this.Ok(await this.postService.GetPostsByGroupId(groupId));
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving group's posts: {ex.Message}");
            }
        }

        [HttpGet("user/{userId}/homefeed")]
        public async Task<ActionResult<List<Post>>> GetHomeFeed(int userId)
        {
            try
            {
                var posts = await this.postService.GetPostsHomeFeed(userId);
                return this.Ok(posts);
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error retrieving home feed: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SavePost(Post post)
        {
            try
            {
                if (post == null)
                    return this.BadRequest("Post data cannot be null.");

                await this.postService.AddPost(post.Title, post.Content, post.UserId, post.GroupId, post.Visibility, post.Tag);
                return this.Ok();
            }
            catch (Exception ex)
            {
                return this.StatusCode(404, $"Error saving post: {ex.Message}");
            }
        }

        [HttpGet("{postId}/reactions")]
        public async Task<ActionResult<List<Reaction>>> GetReactionsByPost(long postId)
        {
            return await this.reactionService.GetReactionsByPostId(postId);
        }

        [HttpGet("{postId}/user/{userId}/reaction")]
        public async Task<ActionResult<Reaction>> GetUserPostReaction(int userId, long postId)
        {
            try
            {
                return await this.reactionService.GetReaction(userId, postId);
            }
            catch (Exception ex)
            {
                return this.NotFound($"Reaction not found for user {userId} on post {postId}. Error: {ex.Message}");
            }
        }

        [HttpGet("{postId}/comments")]
        public async Task<ActionResult<List<Comment>>> GetCommentsByPostId(long postId)
        {
            var comments = await this.commentService.GetCommentsByPostId(postId);
            return this.Ok(comments);
        }
    }
}
