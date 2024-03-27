using Microsoft.AspNetCore.Identity;

namespace 常驻后台的数据导出服务
{
    public class User : IdentityUser<long>
    {
        public DateTime CreateTime { get; set; }
    }
}
