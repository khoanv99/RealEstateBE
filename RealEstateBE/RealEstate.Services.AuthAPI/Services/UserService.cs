
using Microsoft.IdentityModel.Tokens;
using RealEstate.Services.AuthAPI.Model;
using RealEstate.Services.AuthAPI.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
namespace RealEstate.Services.AuthAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IDbService _dbService;

        public UserService(IDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<bool> CreateUser(User user)
        {
            var result =
                await _dbService.EditData(
                    "INSERT INTO [User] (Username, Fullname, Email, PasswordHash, PasswordSalt) VALUES(@Username, @Fullname, @Email, @PasswordHash, @PasswordSalt)",
                    user);
            return true;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var deleteEmployee = await _dbService.EditData("DELETE FROM [User] WHERE Id=@userId", new { userId });
            return true;
        }

        public async Task<User> GetUser(int id)
        {
            var userList = await _dbService.GetAsync<User>("SELECT * FROM [User] where Id=@id", new { id });
            return userList;
        }


        public async Task<List<User>> GetUserList()
        {
            var userList = await _dbService.GetAll<User>("SELECT * FROM [User]");
            return userList;
            //throw new NotImplementedException();
        }

        public Task<User> UpdateInfoUser(User user)
        {
            throw new NotImplementedException();
        }

        public async Task<User> CheckUserWithRole(string username)
        {
            var userList = await _dbService.GetAsync<User>("SELECT U.*, R.Code AS RoleCode FROM [User] U JOIN UserRole UR ON U.Id = UR.UserId JOIN Role R ON UR.RoleId = R.Id WHERE U.Username = @username;", new { username });
            return userList;
        }
        public async Task<User> CheckAvaiableUser(string username)
        {
            var userList = await _dbService.GetAsync<User>("SELECT * FROM [User] WHERE Username = @username;", new { username });
            return userList;
        }

        public async Task<bool> CreateRole(Role role)
        {
            var result =
                    await _dbService.EditData(
                        "INSERT INTO [Role] (Code, Description) VALUES(@Code, @Description)",
                        role);
            return true;
        }

        public async Task<Role> CheckAvaiableRole(string CodeRole)
        {
            var roleList = await _dbService.GetAsync<Role>("SELECT * FROM [Role] where Code=@CodeRole ", new { CodeRole });
            return roleList;
        }

        public async Task<bool> AddRoleForUser(int userid, int roleid)
        {
            var result =
                await _dbService.EditData(
                    "INSERT INTO [UserRole] (UserId, RoleId) VALUES(@userid, @roleid)", new { userid, roleid });
            return result != -1 ? true : false;
        }

        public async Task<bool> UpdatePassword(User user)
        {
            var updateuser =
           await _dbService.EditData(
               "Update [User] SET PasswordHash=@PasswordHash, PasswordSalt=@PasswordSalt WHERE Username=@Username",
               user);
            return updateuser != -1 ? true : false;
        }

        public async Task<bool> UpdateFullName(string username, string fullname)
        {
            var updateuser =
                      await _dbService.EditData(
                          "Update [User] SET Fullname=@fullname WHERE Username=@username",
                         new { username, fullname });
            return updateuser != -1 ? true : false;
        }
        public async Task<bool> RemoveRoleUser(int userId, int roleId)
        {
            var result = await _dbService.EditData("DELETE FROM [UserRole] WHERE UserId=@userId AND RoleId=@roleId", new { userId, roleId });
            return result != -1 ? true : false;
        }
    }
}
