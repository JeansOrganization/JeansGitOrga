using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace FluentValidation的基本使用
{
    public class Login2RequestValidator : AbstractValidator<Login2Request>
    {
        public Login2RequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull().WithMessage("邮箱不能为空")
                .EmailAddress().WithMessage("不是正确的邮箱地址")
                .Must(r => r.EndsWith("qq.com") || r.EndsWith("163.com")).WithMessage("您的邮箱不是qq邮箱或者网易邮箱");
            RuleFor(x => x.password)
                .NotNull().WithMessage("密码不能为空")
                .Length(8, 16).WithMessage("密码必须是8到16位");
            RuleFor(x=>x.password2)
                .Equal(r=>r.password).WithMessage("密码必须一致");
        }
    }
}
