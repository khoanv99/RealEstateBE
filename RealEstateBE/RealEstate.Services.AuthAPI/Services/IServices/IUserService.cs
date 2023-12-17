using RealEstate.Services.AuthAPI.Model;

namespace RealEstate.Services.AuthAPI.Services.IServices
{
    public interface IUserService
    {
        Task<bool> CreateUser(User user);
        Task<bool> CreateRole(Role role);
        Task<bool> AddRoleForUser(int userid, int roleid);
        Task<bool> UpdatePassword(User user);
        Task<bool> UpdateFullName(string username, string fullname);
        Task<User> UpdateInfoUser(User user);
        Task<User> GetUser(int id);
        Task<User> CheckUserWithRole(string username);
        Task<User> CheckAvaiableUser(string username);
        Task<List<User>> GetUserList();
        Task<Role> CheckAvaiableRole(string CodeRole);
        Task<bool> DeleteUser(int userId);
        Task<bool> RemoveRoleUser(int userId, int roleId);
    }
}
