namespace Workout.Core.IServices
{
    using Workout.Core.Enums;
    using Workout.Core.Models;

    public interface IPostService
    {
        Task AddPost(string title, string content, int userId, long? groupId, PostVisibility postVisibility, PostTag postTag);

        //void DeletePost(long id);

        Task<List<Post>> GetPostsByUserId(int userId);

        Task<List<Post>> GetAllPosts();

        Task<List<Post>> GetPostsByGroupId(long groupId);

        Task<Post> GetPostById(long id);

        //List<Post> GetPostsGroupsFeed(long userId);

        Task<List<Post>> GetPostsHomeFeed(int userId);

        //void UpdatePost(long id, string title, string description, PostVisibility visibility, PostTag tag);

        //void SavePost(Post entity);
    }
}