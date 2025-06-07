namespace Workout.Core.IServices
{
    using ServerLibraryProject.Models;

    public interface ICommentService
    {
        Task<List<Comment>> GetAllComments();

        //Comment GetCommentById(int commentId);

        Task<List<Comment>> GetCommentsByPostId(long postId);

        Task<Comment> AddComment(string content, int userId, long postId);

        //void DeleteComment(long commentId);

        //void UpdateComment(long commentId, string content);
    }
}