using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Identity标识框架.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;

        public TestController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(ResetPasswordByOldRequest req)
        {
            string username = req.username;
            string password = req.password;
            string oldpassword = req.oldpassword;
            var r = await Login(new LoginRequest(username, oldpassword));
            if (r.Value == "登录成功")
            {
                var user = await userManager.FindByNameAsync(username);
                string token = await userManager.GeneratePasswordResetTokenAsync(user);
                var result = await userManager.ResetPasswordAsync(user, token, password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return Ok("修改成功");
            }
            return BadRequest("密码验证错误");
        }

        [HttpPost]
        public async Task<ActionResult> InitAdminAndUser()
        {
            var roleExist = await roleManager.RoleExistsAsync("admin");
            if (!roleExist)
            {
                Role role = new Role() { Name = "admin" };
                var r = await roleManager.CreateAsync(role);
                if (!r.Succeeded) return BadRequest(r.Errors);
            }
            var user = await userManager.FindByNameAsync("jean");
            if(user == null)
            {
                user = new User() { UserName = "jean", Email = "771058475@qq.com",EmailConfirmed = true };
                var r = await userManager.CreateAsync(user,"123456");
                if (!r.Succeeded) return BadRequest(r.Errors);
                var result = await userManager.AddToRoleAsync(user, "admin");
                if (!result.Succeeded) return BadRequest(result.Errors);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginRequest req)
        {
            string username = req.username;
            string password = req.password;
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return NotFound("用户不存在:" + username);
            if (await userManager.IsLockedOutAsync(user)) return BadRequest("正在锁定中，请稍后重试");
            var checkPassword = await userManager.CheckPasswordAsync(user, password);
            if (!checkPassword)
            {
                var r = await userManager.AccessFailedAsync(user);
                return BadRequest("密码错误!!");
            }
            else
            {
                await userManager.ResetAccessFailedCountAsync(user);
                return "登录成功";
            }
        }
    }
}
