namespace Identity标识框架.Controllers
{
    public record ResetPasswordByOldRequest(string username,string password,string oldpassword);
}
