using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SignalR身份认证.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        private readonly UserManager<User> userManager;

        public DemoController(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        private static string BuildToken(List<Claim> claims, JwtOptions jwtOptions)
        {
            DateTime notBefore = DateTime.UtcNow;
            DateTime expires = notBefore.AddSeconds(jwtOptions.ExpireSeconds);
            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey!));
            var Credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken("Poster", "Getter", claims, notBefore, expires, Credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpPost]
        public async Task<ActionResult<ResultResponse<User>>> Login(LoginRequest req, [FromServices] IOptionsSnapshot<JwtOptions> jwtOptions)
        {
            string username = req.username;
            string password = req.password;
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest(new ResultResponse<object>("-1","不存在该用户"));
            if (!await userManager.CheckPasswordAsync(user, password)) 
                return BadRequest(new ResultResponse<object>("-1", "密码错误"));


            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName.ToString()));
            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            string jwt = BuildToken(claims, jwtOptions.Value);
            return Ok(new ResultResponse<User>("0", "成功", jwt, user));
        }
    }
}
