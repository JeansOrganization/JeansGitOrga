using Microsoft.AspNetCore.Mvc;

namespace FluentValidation的基本使用.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        [HttpPost]
        public ActionResult Login1(Login2Request req)
        {
            return Ok(req);
        }
    }
}
