using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace WebMud.Data
{
    public class NameUserIdProvider : IUserIdProvider
    {
        public virtual string GetUserId(HubConnectionContext connection)
        {

            return connection.User?.FindFirst(ClaimTypes.Email)?.Value;


        }
    }
}
