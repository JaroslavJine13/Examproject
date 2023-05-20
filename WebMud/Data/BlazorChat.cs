using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using WebMud.Models;

namespace WebMud.Data
{

    public class BlazorChat : Hub
    {

        private readonly IChatService _ChatService;

        public BlazorChat(IChatService chatService)
        {
            _ChatService = chatService;
        }


        public Task JoinGroup(string fromUser, string toUser)
        {


            return Groups.AddToGroupAsync(Context.ConnectionId, toUser);
        }

        public const string HubUrl = "/chat";

        //public async Task Save(Message message)
        //{
        //   _ChatService.AddMessage(message);
        //}


        public async Task Notification(Message message)
        {
            await Clients.All.SendAsync("Notification", message);

        }

        public async Task Broadcast(Message message)
        {

            await _ChatService.AddMessage(message);

            await Clients.All.SendAsync("Broadcast", message);

        }

        public async Task SendPrivateMessage(Message message)
        {

            await _ChatService.AddMessage(message);



            if (message.ToUserId == message.FromUserId)
            {

                await Clients.Group(message.FromUserId).SendAsync("SendPrivateMessage", message);

            }   
            else
            {

                await Clients.Group(message.ToUserId).SendAsync("SendPrivateMessage", message);
                await Clients.Group(message.FromUserId).SendAsync("SendPrivateMessage", message);

            }


        }



        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"{Context.ConnectionId} connected");
                   
                return base.OnConnectedAsync();

        }

        public override async Task OnDisconnectedAsync(Exception e)
        {                    

                Console.WriteLine($"Disconnected {e?.Message} {Context.ConnectionId}");

                await base.OnDisconnectedAsync(e);

        }

    }
    
}
