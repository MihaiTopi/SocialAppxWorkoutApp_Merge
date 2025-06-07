using Workout.Core.Models;

namespace Workout.Core.IRepositories
{
    public interface IGroupRepository
    {
        //void DeleteGroupById(long id);

        Task<List<Group>> GetAllGroups();

        Task<Group> GetGroupById(long id);

        Task<List<Group>> GetGroupsForUser(int userId);

        Task<List<UserModel>> GetUsersFromGroup(long id);

        Task SaveGroup(Group entity);

        //void UpdateGroup(long id, string name, string image, string description, long adminId);
    }
}