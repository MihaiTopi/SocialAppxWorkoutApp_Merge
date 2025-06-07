using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Workout.Core.Models;

namespace NeoIsisJob.Proxy
{
    public class UserServiceProxy : BaseServiceProxy
    {
        private const string EndpointName = "user";

        public UserServiceProxy(IConfiguration configuration = null)
            : base(configuration)
        {
        }

        public async Task<int> RegisterNewUserAsync()
        {
            try
            {
                var result = await PostAsync<int>($"{EndpointName}/register", null);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error registering new user: {ex.Message}");
                throw;
            }
        }

        public async Task<UserModel> GetUserAsync(int userId)
        {
            try
            {
                var result = await GetAsync<UserModel>($"{EndpointName}/{userId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching user: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> RemoveUserAsync(int userId)
        {
            try
            {
                var result = await DeleteAsync<bool>($"{EndpointName}/{userId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing user: {ex.Message}");
                throw;
            }
        }

        public async Task<IList<UserModel>> GetAllUsersAsync()
        {
            try
            {
                var results = await GetAsync<IList<UserModel>>($"{EndpointName}");
                return results ?? new List<UserModel>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all users: {ex.Message}");
                return new List<UserModel>();
            }
        }

        public async Task<UserModel> GetUserByUsername(string username)
        {
            var response = await this.httpClient.GetAsync($"user/user?username={username}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    var user = await response.Content.ReadFromJsonAsync<UserModel>();
                    return user;
                }
            }

            return null;
        }

        public async Task<int> AddUser(string username, string password, string image)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentException("Username cannot be empty", nameof(username));
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be empty", nameof(password));
            }

            var user = new UserModel
            {
                Username = username,
                Password = password,
            };

            var response = await this.httpClient.PostAsJsonAsync(string.Empty, user);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to add user. Status: {response.StatusCode}");
            }

            return -1;
        }

        public async Task<List<UserModel>> GetUserFollowing(int id)
        {
            var client = new HttpClient();
            var response = await client.GetAsync($"http://localhost:5261/api/user/{id}/following");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UserModel>>() ?? new List<UserModel>();
            }

            Debug.WriteLine($"Failed to get following for user {id}. Status: {response.StatusCode}");
            return new List<UserModel>();
        }

        public async Task FollowUserById(int userId, int whoToFollowId)
        {
            var client = new HttpClient();
            var response = await client.PostAsJsonAsync($"http://localhost:5261/api/user/{userId}/followers", whoToFollowId);
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to follow. Status: {response.StatusCode}");
            }
        }

        public async Task UnfollowUserById(int userId, int whoToUnfollowId)
        {
            var client = new HttpClient();
            var response = await client.DeleteAsync($"http://localhost:5261/api/user/{userId}/followers/{whoToUnfollowId}");
            if (!response.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Failed to unfollow. Status: {response.StatusCode}");
            }
        }

        public async Task JoinGroup(int userId, long groupId)
        {
            var client = new HttpClient();

            var response = await client.PostAsJsonAsync($"http://localhost:5261/api/user/{userId}/groups/{groupId}", string.Empty);

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new Exception($"Failed to join group: {response.StatusCode}");
        }

        public async Task ExitGroup(int userId, long groupId)
        {
            var client = new HttpClient();

            var response = await client.DeleteAsync($"http://localhost:5261/api/user/{userId}/groups/{groupId}");

            if (response.IsSuccessStatusCode)
            {
                return;
            }

            throw new Exception($"Failed to exit group: {response.StatusCode}");
        }
    }
}