using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Services.AuthAPI.Model;
using RealEstate.Services.AuthAPI.Services.IServices;

namespace RealEstate.Services.AuthAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        public static Role role = new Role();
        public static UserRole userRole = new UserRole();

        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public ApiResponse _apiResponse;

        public UserRoleController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }
        [HttpPost("createNewRole"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> CreateRole(string Code, string Description)
        {
            var checkRoleResult = await _userService.CheckAvaiableRole(Code);
            if (checkRoleResult != null)
            {
                role.Code = Code;
                role.Description = Description;
                var result = await _userService.CreateRole(role);
                _apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = "Create new role success",
                    Data = role
                };
            }
            else
            {
                _apiResponse = new ApiResponse
                {
                    Success = false,
                    Message = "Role is Avaiable"
                };
            }
            return Ok(_apiResponse);
        }

        [HttpPost("addRoleForUser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> AddRoleForUser(int userid, int roleid)
        {
            var result = await _userService.AddRoleForUser(userid, roleid);
            if (result)
            {
                _apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = "Add role for user success"
                };
            }
            else

            {
                _apiResponse = new ApiResponse
                {
                    Success = false,
                    Message = "username or role is not valid"
                };
            }
            return Ok();
        }

        [HttpDelete("removeRoleUser"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<User>> RemoveRoleUser(int userid, int roleid)
        {
            var result = await _userService.RemoveRoleUser(userid, roleid);
            if (result)
            {
                _apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = "Remove role for user success",
                    Data = userRole
                };
            }
            else
            {
                _apiResponse = new ApiResponse
                {
                    Success = false,
                    Message = "not found userid or roleid"
                };
            }
            return Ok();
        }

    }
}
