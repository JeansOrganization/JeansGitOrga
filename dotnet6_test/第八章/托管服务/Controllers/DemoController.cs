using Microsoft.AspNetCore.Mvc;

namespace 托管服务.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }
    }
}
