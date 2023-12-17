using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Services.AuthAPI.Model;
using RealEstate.Services.AuthAPI.Services.IServices;
using System.Security.Cryptography;

namespace RealEstate.Services.AuthAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public ApiResponse _apiResponse;

        public UserController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<User>> Register(string username, string email, string password, string confirmPassword)
        {
            var checkisValiUser = await _userService.CheckAvaiableUser(username);
            if (checkisValiUser == null)
            {
                if (password.Equals(confirmPassword))
                {
                    CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

                    user.Username = username;
                    user.Email = email;
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    user.RoleCode = "Normal";
                    var result = await _userService.CreateUser(user);
                    _apiResponse = new ApiResponse
                    {
                        Success = true,
                        Message = "Register success",
                        Data = user
                    };
                }
                else
                {
                    _apiResponse = new ApiResponse
                    {
                        Success = false,
                        Message = "Password not equal confirm password!"
                    };
                }
            }
            else
            {
                _apiResponse = new ApiResponse
                {
                    Success = false,
                    Message = "User is Valid"
                };
            }
            return Ok(_apiResponse);

        }

        [HttpGet]
        public async Task<List<User>> getAllUser()
        {
            var userList = await _userService.GetUserList();
            return userList;

        }

        [HttpDelete("{id:int}"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var userList = await _userService.GetUser(id);
            if (userList != null)
            {
                var result = await _userService.DeleteUser(id);
                if (result == true)
                {
                    _apiResponse = new ApiResponse
                    {
                        Success = true,
                        Message = "Delete success id is " + id.ToString(),
                    };
                }
            }
            else
            {
                _apiResponse = new ApiResponse
                {
                    Success = false,
                    Message = "Not found user"
                };
            }

            return Ok(_apiResponse);
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }


    }
}
