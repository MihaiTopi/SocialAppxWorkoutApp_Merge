using Workout.Core.IRepositories;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Core.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository groupRepository;
        private readonly IUserRepo userRepository;

        public GroupService(IGroupRepository groupRepository, IUserRepo userRepository)
        {
            this.groupRepository = groupRepository;
            this.userRepository = userRepository;
        }

        public async Task<Group> GetGroupById(long id)
        {
            return await this.groupRepository.GetGroupById(id);
        }

        public async Task<List<Group>> GetUserGroups(int userId)
        {
            return await this.groupRepository.GetGroupsForUser(userId);
        }

        public async Task<List<UserModel>> GetUsersFromGroup(long groupId)
        {
            return await this.groupRepository.GetUsersFromGroup(groupId);
        }

        public async Task<Group> AddGroup(string name, string desc)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Group name cannot be empty");
            }

            var group = new Group
            {
                Name = name,
                Description = desc,
            };

            await this.groupRepository.SaveGroup(group);
            return group;
        }

        //public void DeleteGroup(long groupId)
        //{
        //    if (groupRepository.GetGroupById(groupId) == null)
        //    {
        //        throw new ArgumentException("Group does not exist");
        //    }

        //    groupRepository.DeleteGroupById(groupId);
        //}

        //public void UpdateGroup(long id, string name, string desc, string image, long adminId)
        //{
        //    if (string.IsNullOrEmpty(name))
        //    {
        //        throw new ArgumentException("Group name cannot be empty");
        //    }

        //    var group = groupRepository.GetGroupById(id);
        //    if (group == null)
        //    {
        //        throw new ArgumentException($"Group with ID {id} does not exist");
        //    }

        //    if (userRepository.GetById(adminId) == null)
        //    {
        //        throw new ArgumentException($"User with ID {adminId} does not exist");
        //    }

        //    try
        //    {
        //        groupRepository.UpdateGroup(id, name, image, desc, adminId);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Failed to update group: {ex.Message}");
        //    }
        //}

        public async Task<List<Group>> GetAllGroups()
        {
            return await this.groupRepository.GetAllGroups();
        }
    }
}