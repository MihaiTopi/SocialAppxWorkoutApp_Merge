using Workout.Core.Models;

namespace ServerLibraryProject.Interfaces
{
    public interface IPostRepository
    {
        //bool DeletePostById(long postId);
        Task<List<Post>> GetAllPosts();
        Task<List<Post>> GetPostsByGroupId(long groupId);
        Task<Post> GetPostById(long postId);
        Task<List<Post>> GetPostsByUserId(int userId);
        Task<List<Post>> GetPostsGroupsFeed(int userId);
        Task<List<Post>> GetPostsHomeFeed(int userId);
        Task SavePost(Post entity);
        //bool UpdatePostById(long postId, string title, string content, PostVisibility visibility, PostTag tag);
    }
}