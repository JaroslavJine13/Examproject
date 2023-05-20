using System.Collections.Generic;
using System.Threading.Tasks;
using WebMud.Models;

namespace WebMud.Data
{
    public interface IUsersService
    {

        Task<List<Users>> GetUsers();

        Task<List<UsersNotified>> GetChatUsers(string toUserId);

        Task<List<UsersNotified>> GetChatUsersAll(string toUserId);

        Task<List<Users>> GetUsersIncludeDeleted();

        Task<Users> GetUsers(Guid id);

        Task<Users> GetUsers(string email);

        Task<bool> UpdateUsers(Users users);

        Task<bool> VerifyUser(Guid id);

        Task<bool> AddUsers(Users users, string baseuri);

        Task<bool> DeleteUsers(Guid id);

        bool IfExists(string email);

        bool IfExists(Guid id);

        Task<bool> UpdatePass(string pass, string name);

        bool VerifyPass(string pass, string name);

        Task<bool> InsertImage(string path, string name);

        Task<bool> ResetUsersPass(Guid id, string email, string baseuri);

        Task<bool> ResetUsersPassAssignPwd(Guid id);

        Task<bool> ResendToken(Users users, string baseuri);

        Task<bool> AddGusetUser(Users users);

        Task<bool> UpdatePublicChatVisibility(bool res, Guid id);

        Task<bool> UpdateDarkMode(bool res, Guid id);
    }
}
