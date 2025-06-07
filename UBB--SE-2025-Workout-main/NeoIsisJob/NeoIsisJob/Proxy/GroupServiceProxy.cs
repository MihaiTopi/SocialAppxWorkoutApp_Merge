
namespace DesktopProject.Proxies
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Workout.Core.IServices;
    using Workout.Core.Models;

    public class GroupServiceProxy : IGroupService
    {
        private readonly HttpClient httpClient;

        public GroupServiceProxy()
        {
            this.httpClient = new HttpClient();

            this.httpClient.BaseAddress = new Uri("http://localhost:5261/api/groups/");
        }

        public async Task<Group> GetGroupById(long id)
        {
            var response = await this.httpClient.GetAsync($"{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Group>();
            }


            throw new Exception($"Failed to get group: {response.StatusCode}");
        }

        public async Task<List<Group>> GetUserGroups(int userId)
        {
            var response = await this.httpClient.GetAsync($"{userId}/groups");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Group>>();
            }
            throw new Exception($"Failed to get groups: {response.StatusCode}");
        }

        public async Task<List<UserModel>> GetUsersFromGroup(long groupId)
        {

            var response = await this.httpClient.GetAsync($"{groupId}/users");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UserModel>>();
            }

            throw new Exception($"Failed to get users from group: {response.StatusCode}");
        }

        public async Task<Group> AddGroup(string name, string desc)
        {
            var group = new Group
            {
                Name = name,
                Description = desc,
            };

            var response = await this.httpClient.PostAsJsonAsync(string.Empty, group);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Group>();
            }
            throw new Exception($"Failed to add group: {response.StatusCode}");
        }

        //public void DeleteGroup(long groupId)
        //{
        //    var response = this.httpClient.DeleteAsync($"{groupId}").Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to delete group: {response.StatusCode}");
        //    }
        //}

        //public void UpdateGroup(long id, string name, string desc, string image, long adminId)
        //{
        //    var group = new Group
        //    {
        //        Name = name,
        //        Description = desc,
        //        Image = image,
        //        AdminId = adminId,
        //    };

        //    var response = this.httpClient.PutAsJsonAsync($"{id}", group).Result;
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        throw new Exception($"Failed to update group: {response.StatusCode}");
        //    }
        //}

        public async Task<List<Group>> GetAllGroups()
        {
            var response = await this.httpClient.GetAsync(string.Empty);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<Group>>();
            }

            throw new Exception($"Failed to get all groups: {response.StatusCode}");
        }
    }
}
