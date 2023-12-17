using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Services.AuthAPI.Model;
using RealEstate.Services.AuthAPI.Services.IServices;
using System.Security.Claims;
using System.Security.Cryptography;

namespace RealEstate.Services.AuthAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserInfoController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public ApiResponse _apiResponse;

        public UserInfoController(IConfiguration configuration, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
        }

        [HttpPut("changePassword"), Authorize]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string confirmedPassword)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;
            var userList = await _userService.CheckAvaiableUser(username);

            if (!newPassword.Equals(confirmedPassword))
            {
                return BadRequest("newPassword is not confirmedPassword");
            }

            if (userList?.Username != username)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(oldPassword, userList.PasswordHash, userList.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            //update password
            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.Username = username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            var checkupdate = await _userService.UpdatePassword(user);
            if (checkupdate)
            {
                _apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = "Update password success",
                };
            }
            return Ok(_apiResponse);
        }

        [HttpPut("changeFullName"), Authorize]
        public async Task<IActionResult> ChangeFullname(string fullname)
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value;

            var checkupdate = await _userService.UpdateFullName(username, fullname);
            if (checkupdate)
            {
                _apiResponse = new ApiResponse
                {
                    Success = true,
                    Message = "Update fullname success",
                };
            }
            return Ok(_apiResponse);
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
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

