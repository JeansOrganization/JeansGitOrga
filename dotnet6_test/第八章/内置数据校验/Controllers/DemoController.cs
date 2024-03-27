using Microsoft.AspNetCore.Mvc;

namespace 内置数据校验.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        [HttpPost]
        public ActionResult Login1(Login1Request req)
        {
            return Ok(req);
        }
    }
}
