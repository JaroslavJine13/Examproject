using WebMud.Models;
using WebMud.Shared;

namespace WebMud.Data
{
    public interface IChatService
    {

        Task<bool> AddMessage(Message message);

        Task<List<Message>> GetNoticeMessage(string toUser);

        Task<bool> ClearMessage(string toUser, string fromUser);

        Task<List<Message>> GetMessage(string toUser, string fromUser);

        Task<bool> ChangeNotice(string toUser,string fromUser);

    }
}