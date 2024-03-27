using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class Demo2Controller : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Hello()
        {
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string userName = this.User.FindFirst(ClaimTypes.Name)!.Value;
            IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
            string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
            return Ok($"id={id},userName={userName},roleNames ={roleNames}");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Hello2()
        {
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string userName = this.User.FindFirst(ClaimTypes.Name)!.Value;
            IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
            string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
            return Ok($"id={id},userName={userName},roleNames ={roleNames}");
        }

        [HttpGet]
        public IActionResult Hello3()
        {
            string id = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            string userName = this.User.FindFirst(ClaimTypes.Name)!.Value;
            IEnumerable<Claim> roleClaims = this.User.FindAll(ClaimTypes.Role);
            string roleNames = string.Join(',', roleClaims.Select(c => c.Value));
            return Ok($"id={id},userName={userName},roleNames ={roleNames}");
        }
    }
}
