using Microsoft.AspNetCore.Mvc;

namespace 客户端响应缓存.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    //[ResponseCache(Duration = 30, Location = ResponseCacheLocation.Client)]
    public class TestController : ControllerBase
    {
        //设置客户端缓存，间隔15秒释放并重新设置缓存
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Client)]
        [HttpGet]
        public ActionResult<DateTime> GetDateNow()
        {
            return DateTime.Now;
        }

        [ResponseCache(Duration = 60)]
        [HttpGet]
        public ActionResult<DateTime> GetDateNowPost()
        {
            return DateTime.Now;
        }
    }
}
