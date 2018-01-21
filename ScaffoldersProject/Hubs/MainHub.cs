using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using ScaffoldersProject.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScaffoldersProject.Hubs
{
    public class MainHub:Hub
    {
        //To provide you a quick overview, the Hub is the center piece of the SignalR. 
        //Similar to the Controller in ASP.NET MVC, a Hub is responsible for receiving 
        //input and generating the output to the client.
        private readonly UserManager<ApplicationUser> _userManager;
        //Dioctionary with Key=userId and Value=connectionId
        private ConcurrentDictionary<string, string> OnlineUser { get; set; }
        static int NumberOfUsers = 0;

        //Depedency injection
        public MainHub(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        //This happens on connection
        public async override Task OnConnectedAsync()
        {
            NumberOfUsers += 1;
            OnlineUser = new ConcurrentDictionary<string, string>();
            OnlineUser.AddOrUpdate(_userManager.GetUserId(Context.User), Context.ConnectionId, (key, value) => Context.ConnectionId);
            //list of online users
            var onUsers = OnlineUser.Keys;
            //Return user name and total login users
            await Clients.All.InvokeAsync("Send", $"{_userManager.GetUserName(Context.User)} joined", NumberOfUsers);
            //return name of online users
            await Clients.Client(Context.ConnectionId).InvokeAsync("OnlineUsers", onUsers);
        }

        //When the client disconnect
        public override Task OnDisconnectedAsync(Exception exception)
        {
            NumberOfUsers -= 1;
            return Clients.All.InvokeAsync("Send", $"{Context.ConnectionId} left", NumberOfUsers);
        }

        //When we invoke from client with Send value end send back message from parameter
        public async Task Send(string message)
        {
            await Clients.All.InvokeAsync("Send", message, NumberOfUsers);
        }

        //When we invoke from client with SendClient value
        public Task SendClient(string message)
        {
            return Clients.Client(Context.ConnectionId).InvokeAsync("Client", "Malaka");
        }

    }
}
