namespace ServerAPIProject.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using ServerLibraryProject.Models;
    using Workout.Core.IServices;

    [ApiController]
    [Route("api/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;

        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Comment>>> GetAllComments()
        {
            var comments = await this.commentService.GetAllComments();
            return this.Ok(comments);
        }

        //[HttpGet("{id}")]
        //public ActionResult<List<Comment>> GetCommentById(long id)
        //{
        //    var comment = this.commentService.GetCommentsByPostId((int)id);
        //    if (comment == null)
        //    {
        //        return this.NotFound($"Comment with ID {id} not found.");
        //    }

        //    return this.Ok(comment);
        //}

        [HttpPost]
        public async Task<IActionResult> SaveComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return this.BadRequest("Comment cannot be null.");
            }

            return this.Ok(await this.commentService.AddComment(comment.Content, comment.UserId, comment.PostId));
        }
    }
}
