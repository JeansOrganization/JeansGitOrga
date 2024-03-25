using Microsoft.AspNetCore.Mvc;

namespace 请求限流器.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetData()
        {
            return "成功获取数据";
        }
    }
}
