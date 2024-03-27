using Microsoft.AspNetCore.SignalR;

namespace SignalR基本使用
{
    public class CharHub : Hub
    {
        public Task SendPublicMessage(string msg)
        {
            var connectionId = this.Context.ConnectionId;
            string message = $"{connectionId}--{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm:ss")}--{msg}";
            return this.Clients.All.SendAsync("ReceiptPublicMessage", message);
        }
    }
}
