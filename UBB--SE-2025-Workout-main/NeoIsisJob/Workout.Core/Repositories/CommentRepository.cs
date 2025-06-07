namespace ServerLibraryProject.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using ServerLibraryProject.Interfaces;
    using ServerLibraryProject.Models;
    using Workout.Core.Data;

    public class CommentRepository : ICommentRepository
    {
        private readonly WorkoutDbContext dbContext;

        public CommentRepository(WorkoutDbContext context)
        {
            this.dbContext = context;
        }

        /// <summary>
        /// Retrieves all comments from the database.
        /// </summary>
        /// <returns>A list of all Comment entities in the system.</returns>
        public Task<List<Comment>> GetAllComments()
        {
            return this.dbContext.Comments.ToListAsync();
        }

        /// <summary>
        /// Retrieves all comments associated with a specific post.
        /// </summary>
        /// <param name="postId">The ID of the post to retrieve comments for.</param>
        /// <returns>A list of Comment entities for the specified post.</returns>
        public Task<List<Comment>> GetCommentsByPostId(long postId)
        {
            return this.dbContext.Comments.Where(c => c.PostId == postId).ToListAsync();
        }

        /// <summary>
        /// Deletes a comment from the database by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to delete.</param>
        //public void DeleteCommentById(long id)
        //{
        //    try
        //    {
        //        var comment = this.dbContext.Comments.Find(id);
        //        if (comment != null)
        //        {
        //            this.dbContext.Comments.Remove(comment);
        //            this.dbContext.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //        throw new Exception("Comment not found or could not be deleted.");
        //    }
        //}

        /// <summary>
        /// Retrieves a single comment by its ID.
        /// </summary>
        /// <param name="id">The ID of the comment to retrieve.</param>
        /// <returns>The Comment entity with the specified ID, or null if not found.</returns>
        //public Comment GetCommentById(long id)
        //{
        //    var comment = this.dbContext.Comments.Find(id);
        //    return comment ?? throw new Exception("Comment not found.");
        //}

        /// <summary>
        /// Saves a new comment to the database.
        /// </summary>
        /// <param name="entity">The Comment entity to be saved.</param>
        public async Task SaveComment(Comment entity)
        {
            try
            {
                await this.dbContext.Comments.AddAsync(entity);
                await this.dbContext.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Error saving comment. Please try again later.");
            }

            return;
        }

        /// <summary>
        /// Updates the content of an existing comment.
        /// </summary>
        /// <param name="id">The ID of the comment to update.</param>
        /// <param name="content">The new content for the comment.</param>
        //public void UpdateCommentContentById(long id, string content)
        //{
        //    try
        //    {
        //        var comment = this.dbContext.Comments.Find(id);
        //        if (comment != null)
        //        {
        //            comment.Content = content;
        //            this.dbContext.SaveChanges();
        //        }
        //    }
        //    catch
        //    {
        //        throw new Exception("Comment not found or could not be updated.");
        //    }
        //}
    }
}