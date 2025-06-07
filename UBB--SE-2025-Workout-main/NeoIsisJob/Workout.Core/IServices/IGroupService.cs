using Workout.Core.Models;

namespace Workout.Core.IServices
{
    public interface IGroupService
    {
        Task<Group> GetGroupById(long id);

        Task<List<Group>> GetUserGroups(int userId);

        Task<List<UserModel>> GetUsersFromGroup(long groupId);

        Task<Group> AddGroup(string name, string desc);

        // void DeleteGroup(long groupId);

        // void UpdateGroup(long id, string name, string desc, string image, long adminId);
        Task<List<Group>> GetAllGroups();
    }
}