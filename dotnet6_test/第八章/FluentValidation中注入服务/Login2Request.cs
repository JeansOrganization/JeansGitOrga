using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation中注入服务
{
    public record Login2Request(string username,string Email,string password,string password2);
}
