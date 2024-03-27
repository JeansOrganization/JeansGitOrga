using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace 常驻后台的数据导出服务.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly ILogger<DemoController> logger;
        private readonly RoleManager<Role> roleManager;

        public DemoController(UserManager<User> userManager, ILogger<DemoController> logger, RoleManager<Role> roleManager)
        {
            this.userManager = userManager;
            this.logger = logger;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public async Task<ActionResult> InsertAnyUser()
        {
            var roleExist = await roleManager.RoleExistsAsync("user");
            if (!roleExist)
            {
                Role role = new Role() { Name = "user" };
                var r = await roleManager.CreateAsync(role);
                if (!r.Succeeded) return BadRequest(r.Errors);
            }
            int addDay = Random.Shared.Next(10);
            string username = "Jean" + DateTime.Now.ToString("ddHHmmss");
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new User()
                {
                    UserName = "Jean" + DateTime.Now.ToString("ddHHmmss"),
                    Email = "771058475@qq.com",
                    CreateTime = DateTime.Now.AddDays(addDay)
                };
                await userManager.CreateAsync(user, "123456");
                await userManager.AddToRoleAsync(user,"user");
                return Ok("ok");
            }
            return BadRequest("fail");
        }
    }
}
