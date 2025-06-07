//localhost/posts
namespace ServerMVCProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Services;
    using System.Diagnostics;
    using Workout.Core.Enums;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Core.Repositories;
    using Workout.Web.Filters;
    using Workout.Web.ViewModels;
    using Workout.Web.ViewModels.PostViewModels;

    [Route("posts")]
    [AuthorizeUser]
    public class ViewPostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IReactionRepository reactionRepository;
        private readonly ICommentService commentService;
        private readonly IUserService userService;

        public ViewPostsController(IPostService postService, IReactionRepository reactionRepository, ICommentService commentService, IUserService userService)
        {
            this.postService = postService;
            this.reactionRepository = reactionRepository;
            this.commentService = commentService;
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            try
            {
                string userIdStr = this.HttpContext.Session.GetString("UserId");
                if (!int.TryParse(userIdStr, out int userId))
                {
                    return Forbid();
                }

                int pageSize = 6;

                // Get all posts for the feed
                List<Post> allPosts = await this.postService.GetPostsHomeFeed(userId);

                // Pagination: skip/take
                var pagedPosts = allPosts
                    .OrderByDescending(p => p.Id)
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var viewModels = new List<PostViewModel>();

                foreach (var post in pagedPosts)
                {
                    var reactions = await reactionRepository.GetReactionsByPostId(post.Id);
                    var comments = await commentService.GetCommentsByPostId(post.Id);
                    var author = await userService.GetUserAsync(post.UserId);

                    var commentsForPost = new List<CommentViewModel>();
                    foreach (var comment in comments)
                    {
                        var commentAuthor = await userService.GetUserAsync(comment.UserId);
                        commentsForPost.Add(new CommentViewModel
                        {
                            Comment = comment,
                            Author = commentAuthor
                        });
                    }

                    viewModels.Add(new PostViewModel
                    {
                        Post = post,
                        Author = author,
                        Reactions = reactions,
                        Comments = commentsForPost
                    });
                }

                // Pass pagination data to ViewBag or ViewData for use in view
                ViewBag.CurrentPage = page;
                ViewBag.TotalPages = (int)Math.Ceiling(allPosts.Count / (double)pageSize);
                ViewBag.UserId = userId;

                return View(viewModels);
            }
            catch (Exception ex)
            {
                // Log error here
                return Forbid();
            }
        }




        [HttpPost("react")]
        [AuthorizeUser]
        public async Task<JsonResult> ReactAjax(long postId, string type)
        {
            try
            {
                int userId = 1; // Hardcoded user ID for testing
                if (HttpContext.Session.GetString("UserId") != null)
                    userId = int.Parse(HttpContext.Session.GetString("UserId"));

                if (!Enum.TryParse<ReactionType>(type, out var reactionType))
                    return Json(new { success = false, error = "Invalid reaction type" });

                var existing = await reactionRepository.GetReaction(userId, postId);
                if (existing == null)
                {
                    await reactionRepository.Add(new Reaction { UserId = userId, PostId = postId, Type = reactionType });
                }
                else if (existing.Type == reactionType)
                {
                    await reactionRepository.Delete(userId, postId);
                }
                else
                {
                    await reactionRepository.Update(userId, postId, reactionType);
                }

                var reactions = await reactionRepository.GetReactionsByPostId(postId);
                return Json(new
                {
                    success = true,
                    like = reactions.Count(r => r.Type == ReactionType.Like),
                    love = reactions.Count(r => r.Type == ReactionType.Love),
                    laugh = reactions.Count(r => r.Type == ReactionType.Laugh),
                    anger = reactions.Count(r => r.Type == ReactionType.Anger)
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }



    }
}