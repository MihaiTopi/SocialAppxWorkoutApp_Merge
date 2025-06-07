using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IUserRepo
    {
        Task<UserModel?> GetUserByIdAsync(int userId);

        Task<UserModel?> GetUserByUsernameAsync(string username);

        Task<int> InsertUserAsync();

        Task<int> InsertUserAsync(string username, string email, string password);

        Task<bool> DeleteUserByIdAsync(int userId);

        Task<List<UserModel>> GetAllUsersAsync();

        Task<List<UserModel>> GetUserFollowers(int id);

        Task<List<UserModel>> GetUserFollowing(int id);

        Task<UserModel> Save(UserModel entity);

        Task Unfollow(int userId, int whoToUnfollowId);

        Task JoinGroup(int userId, long groupId);

        Task ExitGroup(int userId, long groupId);

        Task Follow(int userId, int whoToFollowId);
    }
}
