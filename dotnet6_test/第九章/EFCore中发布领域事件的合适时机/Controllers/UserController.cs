using Microsoft.AspNetCore.Mvc;

namespace EFCore中发布领域事件的合适时机.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserDbContext dbContext;
        private readonly ILogger<UserController> logger;

        public UserController(UserDbContext dbContext, ILogger<UserController> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(AddUserRequest req)
        {
            User user = new User(req.UserName,req.Email);
            dbContext.Users.Add(user);
            await dbContext.SaveChangesAsync();
            return Ok("成功添加用户");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(Guid id,UpdateUserRequest req)
        {
            var user = dbContext.Users.FirstOrDefault(r => r.Id == id);
            if (user == null) return NotFound("没找到用户");
            user.ChangeNickName(req.NickName);
            user.ChangeEmail(req.Email);
            user.ChangeAge(req.Age);
            await dbContext.SaveChangesAsync();
            return Ok("成功更新用户");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = dbContext.Users.FirstOrDefault(r => r.Id == id);
            if (user == null) return NotFound("没找到用户");
            user.SoftDelete();
            await dbContext.SaveChangesAsync();
            return Ok("成功软删除用户");
        }
    }
}
