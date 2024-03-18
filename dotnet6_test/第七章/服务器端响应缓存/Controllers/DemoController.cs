using Microsoft.AspNetCore.Mvc;

namespace 服务器端响应缓存.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    [ResponseCache(CacheProfileName = "jeanCache",Duration = 60)]
    public class DemoController : ControllerBase
    {
        [HttpGet]
        public ActionResult<DateTime> GetDateNow()
        {
            return DateTime.Now;
        }
    }
}
