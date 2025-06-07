//localhost/posts
namespace ServerMVCProject.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Workout.Core.Enums;
    using Workout.Core.IRepositories;
    using Workout.Core.IServices;
    using Workout.Core.Models;
    using Workout.Web.Filters;

    [Route("posts")]
    [AuthorizeUser]
    public class ViewPostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IReactionRepository reactionRepository;

        public ViewPostsController(IPostService postService, IReactionRepository reactionRepository)
        {
            this.postService = postService;
            this.reactionRepository = reactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                string userIdStr = this.HttpContext.Session.GetString("UserId");

                int userId = int.Parse(userIdStr);

                List<Post> posts = await this.postService.GetPostsHomeFeed(userId);
                return this.View(posts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error session");
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