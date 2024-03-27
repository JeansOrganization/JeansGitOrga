using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;

namespace 内置数据校验
{
    public class Login1Request
    {
        [Required]
        [EmailAddress]
        [RegularExpression("^.*@(qq|163)\\.com$", ErrorMessage = "只支持QQ和163邮箱")]
        public string Email { get; set; }
        [Required]
        [StringLength(16,MinimumLength = 8)]
        public string password { get; set; }
        [Compare(nameof(password),ErrorMessage = "两次密码必须相同")]
        public string password2 { get; set; }
    }
}
