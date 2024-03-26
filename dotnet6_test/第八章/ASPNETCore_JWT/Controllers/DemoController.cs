using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ASPNETCore_JWT.Controllers
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

        private static string BuildToken(List<Claim> claims, JwtSetting jwtSetting)
        {
            DateTime notBefore = DateTime.UtcNow;
            DateTime expires = notBefore.AddSeconds(jwtSetting.ExpireSeconds);
            var secKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SigningKey!));
            var Credentials = new SigningCredentials(secKey, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken("Poster", "Getter", claims, notBefore, expires, Credentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpGet]
        public async Task<ActionResult> Login(LoginRequest req, [FromServices] IOptionsSnapshot<JwtSetting> jwtSetting)
        {
            string username = req.username;
            string password = req.password;
            var user = await userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("用户不存在");
            if(await userManager.CheckPasswordAsync(user, password))
            {
                List<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                var roles = await userManager.GetRolesAsync(user);
                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                string jwt = BuildToken(claims,jwtSetting.Value);
                return Ok(jwt);
            }
            else
            {
                return BadRequest("密码错误");
            }
        }
    }
}
