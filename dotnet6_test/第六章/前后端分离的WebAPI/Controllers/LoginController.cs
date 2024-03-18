using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace 前后端分离的WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public ActionResult<LoginResult> Login(LoginRequest request)
        {
            if(request.Username == "admin" && request.Password == "123456")
            {
                var process = Process.GetProcesses().Select(r => new ProcessInfo(r.Id, r.ProcessName, r.WorkingSet64)).ToArray();
                return new LoginResult(true,process);
            }
            return new LoginResult(false, null);
        }
    }
}
