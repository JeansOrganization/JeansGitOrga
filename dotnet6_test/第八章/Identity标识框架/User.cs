using Microsoft.AspNetCore.Identity;

namespace Identity标识框架
{
    public class User : IdentityUser<long>
    {
        public DateTime CreateTime { get; set; }
    }
}
