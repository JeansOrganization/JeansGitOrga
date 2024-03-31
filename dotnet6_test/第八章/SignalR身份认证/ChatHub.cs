using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SignalR身份认证
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly UserManager<User> userManager;
        public ChatHub(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<string> SendPrivateMessage(string toUserName,string msg)
        {
            var user = await userManager.FindByNameAsync(toUserName);
            if (user == null) return "找不到接收方";

            string toID = user.Id.ToString();
            var fromID = this.Context.User!.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var callName = this.Context.User!.FindFirst(ClaimTypes.Name)?.Value;
            string message = $"{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")}--{callName} : {msg}";
            //await this.Clients.User(toID).SendAsync("ReceivePrivateMessage",message,toID);
            //await this.Clients.Users(new List<string>() { toID, fromID }).SendAsync();
            await this.Clients.Users(toID, fromID!).SendAsync("ReceivePrivateMessage", message, toID);
            return "发送成功";
        }
            

        public Task SendPublicMessage(string msg)
        {
            var connectionId = this.Context.ConnectionId;
            string message = $"{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")}--{connectionId} : {msg}";
            return this.Clients.All.SendAsync("ReceivePublicMessage", message);
        }
    }
}
