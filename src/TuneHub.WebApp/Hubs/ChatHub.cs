using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace TuneHub.WebApp.Hubs
{
    public class ChatHub : Hub
    {
        public void SendMessage(string user, string message)
        {
            Clients.All.SendAsync("RecieveMessage", user, message);
        }
    }
}