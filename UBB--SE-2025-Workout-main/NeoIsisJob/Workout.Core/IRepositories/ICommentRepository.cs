using ServerLibraryProject.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface ICommentRepository
    {
        //void DeleteCommentById(long id);

        Task<List<Comment>> GetAllComments();

        //Comment GetCommentById(long id);

        Task<List<Comment>> GetCommentsByPostId(long postId);

        Task SaveComment(Comment entity);

        //void UpdateCommentContentById(long id, string content);
    }
}