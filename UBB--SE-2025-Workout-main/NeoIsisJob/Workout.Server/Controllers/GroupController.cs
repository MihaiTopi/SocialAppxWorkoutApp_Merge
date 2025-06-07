using Microsoft.AspNetCore.Mvc;
using Workout.Core.IServices;
using Workout.Core.Models;

namespace ServerAPIProject.Controllers
{
    [ApiController]
    [Route("api/groups")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService groupService;

        public GroupController(IGroupService groupService)
        {
            this.groupService = groupService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Group>>> GetAllGroups()
        {
            return this.Ok(await groupService.GetAllGroups());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroupById(int id)
        {
            try
            {
                return this.Ok(await this.groupService.GetGroupById(id));

            }
            catch (Exception ex)
            {
                return this.NotFound($"Group with ID {id} not found. Error: {ex.Message}");
            }
        }

        [HttpGet("{id}/users")]
        public async Task<ActionResult<List<UserModel>>> GetUsersFromGroup(int id)
        {
            return this.Ok(await this.groupService.GetUsersFromGroup(id));
        }

        [HttpPost]
        public async Task<IActionResult> SaveGroup([FromBody] Group group)
        {
            try
            {
                var newGroup = await this.groupService.AddGroup(group.Name, group.Description);
                return this.Ok(newGroup);
            }
            catch (Exception ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}/groups")]
        public async Task<ActionResult<List<Group>>> GetGroupsForUser(int userId)
        {
            return Ok(await groupService.GetUserGroups(userId));
        }
    }
}
