// Workout.Server/Controllers/UserController.cs
using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace Workout.Server.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
            => this.userService = userService;

        // GET /api/user/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            try
            {
                UserModel user = await userService.GetUserAsync(userId);
                if (user == null)
                {
                    return NotFound();
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching user: {ex.Message}");
            }
        }

        // GET /api/user
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                IEnumerable<UserModel> users = await userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error fetching users: {ex.Message}");
            }
        }

        // POST /api/user
        [HttpPost]
        public async Task<IActionResult> AddUser()
        {
            try
            {
                // RegisterNewUserAsync returns the new user's ID
                int newUserId = await userService.RegisterNewUserAsync();
                // return 200 OK with the new ID in the body
                return Ok(newUserId);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error adding user: {ex.Message}");
            }
        }

        // DELETE /api/user/{userId}
        [HttpDelete("{userId}")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            try
            {
                await userService.RemoveUserAsync(userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error deleting user: {ex.Message}");
            }
        }

        [HttpGet("user")]
        public async Task<ActionResult<UserModel>> GetUserByUsername([FromQuery] string username)
        {
            try
            {
                return this.Ok(await this.userService.GetUserByUsername(username));
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpPost("{userId}/followers")]
        public async Task<IActionResult> FollowUser(int userId, [FromBody] int followerId)
        {
            try
            {
                await this.userService.FollowUserById(userId, followerId);
                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpDelete("{userId}/followers/{unfollowUserId}")]
        public async Task<IActionResult> UnfollowUser(int userId, int unfollowUserId)
        {
            try
            {
                await this.userService.UnfollowUserById(userId, unfollowUserId);
                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }
        // Add these endpoints inside the UserController class

        /// <summary>
        /// Adds the user to a group.
        /// </summary>
        [HttpPost("{userId}/groups/{groupId}")]
        public async Task<IActionResult> JoinGroup(int userId, long groupId)
        {
            try
            {
                await this.userService.JoinGroup(userId, groupId);
                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Removes the user from a group.
        /// </summary>
        [HttpDelete("{userId}/groups/{groupId}")]
        public async Task<IActionResult> ExitGroup(int userId, long groupId)
        {
            try
            {
                await this.userService.ExitGroup(userId, groupId);
                return this.Ok();
            }
            catch (Exception e)
            {
                return this.BadRequest(e.Message);
            }
        }

        [HttpGet("{userId}/following")]
        public async Task<ActionResult<List<UserModel>>> GetUserFollowing(int userId)
        {
            return await this.userService.GetUserFollowing(userId);
        }
    }
}
