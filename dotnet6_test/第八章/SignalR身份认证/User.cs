using Microsoft.AspNetCore.Identity;

namespace SignalR身份认证
{
    public class User : IdentityUser<long>
    {
        public DateTime CreateTime { get; set; }
    }
}
