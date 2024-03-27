using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation的基本使用
{
    public record Login2Request(string Email,string password,string password2);
}
