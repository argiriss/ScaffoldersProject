using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Models.services
{
    public class ChatHub:Hub
    {
        //To provide you a quick overview, the Hub is the center piece of the SignalR. 
        //Similar to the Controller in ASP.NET MVC, a Hub is responsible for receiving 
        //input and generating the output to the client.
        static int NumberOfUsers=0;

        public override Task OnConnectedAsync()
        {
            NumberOfUsers += 1;
            return Clients.All.InvokeAsync("Send", $"{Context.ConnectionId} joined",NumberOfUsers);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            NumberOfUsers -= 1;
            return Clients.All.InvokeAsync("Send", $"{Context.ConnectionId} left",NumberOfUsers);
        }

        public Task Send(string message)
        {    
            return Clients.All.InvokeAsync("Send", message, NumberOfUsers);
        }
    }
}
