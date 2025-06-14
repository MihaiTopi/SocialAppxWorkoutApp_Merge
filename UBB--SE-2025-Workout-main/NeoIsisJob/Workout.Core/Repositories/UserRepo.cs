﻿using Microsoft.EntityFrameworkCore;
using ServerLibraryProject.DbRelationshipEntities;
using Workout.Core.Data;
using Workout.Core.IRepositories;
using Workout.Core.Models;

namespace Workout.Core.Repositories
{
    public class UserRepo : IUserRepo
    {
        private readonly WorkoutDbContext context;

        public UserRepo(WorkoutDbContext context)
        {
            this.context = context;
        }

        public async Task<UserModel?> GetUserByIdAsync(int userId)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.ID == userId);
        }

        public async Task<UserModel?> GetUserByUsernameAsync(string username)
        {
            return await context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<int> InsertUserAsync()
        {
            var user = new UserModel();
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.ID;
        }

        public async Task<int> InsertUserAsync(string username, string email, string password)
        {
            var user = new UserModel
            {
                Username = username,
                Email = email ?? "",
                Password = password
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user.ID;
        }

        public async Task<bool> DeleteUserByIdAsync(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            context.Users.Remove(user);
            int rowsAffected = await context.SaveChangesAsync();
            return rowsAffected > 0;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<List<UserModel>> GetUserFollowers(int id)

        {
            List<UserModel> userFollowers = new List<UserModel>();
            List<UserFollower> followers = await this.context.UserFollowers
                .Where(uf => uf.UserId == id)
                .ToListAsync();
            foreach (UserFollower userFollower in followers)
            {
                UserModel? user = await context.Users.FirstOrDefaultAsync(u => u.ID == userFollower.FollowerId);
                if (user != null)
                {
                    userFollowers.Add(user);
                }
            }

            return userFollowers;
        }

        public async Task<List<UserModel>> GetUserFollowing(int id)
        {
            var userFollowers = context.UserFollowers
               .Where(uf => uf.UserId == id);
            return await context.Users
                .Where(u => userFollowers.Any(uf => uf.FollowerId == u.ID))
                .ToListAsync();
        }

        public async Task<UserModel> Save(UserModel entity)
        {
            try
            {
                if (await context.Users.FirstOrDefaultAsync(u => u.Username.Equals(entity.Username)) != null)
                {
                    throw new Exception("User already exists");
                }

                await context.Users.AddAsync(entity);
                await context.SaveChangesAsync();
                return entity;
            }
            catch
            {
                throw new Exception("Error saving the user");
            }

        }

        public async Task JoinGroup(int userId, long groupId)
        {
            try
            {
                GroupUser groupUser = new GroupUser
                {
                    UserId = userId,
                    GroupId = groupId
                };
                await context.GroupUsers.AddAsync(groupUser);
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Error joining the group");
            }
        }

        public async Task ExitGroup(int userId, long groupId)
        {
            try
            {
                // Find the GroupUser entry that matches the user and group
                GroupUser? groupUser = await context.GroupUsers
                    .FirstOrDefaultAsync(gu => gu.UserId == userId && gu.GroupId == groupId);

                if (groupUser != null)
                {
                    context.GroupUsers.Remove(groupUser);
                    await context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("User is not a member of the group");
                }
            }
            catch
            {
                throw new Exception("Error exiting the group");
            }
        }

        public async Task Unfollow(int userId, int whoToUnfollowId)
        {
            try
            {
                context.UserFollowers.Remove(new UserFollower(userId, whoToUnfollowId));
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error unfollowing user: " + ex.Message);
            }
        }

        public async Task Follow(int userId, int whoToFollowId)
        {
            try
            {
                await context.UserFollowers.AddAsync(new UserFollower(userId, whoToFollowId));
                await context.SaveChangesAsync();
            }
            catch
            {
                throw new Exception("Error following user");
            }
        }
    }
}
