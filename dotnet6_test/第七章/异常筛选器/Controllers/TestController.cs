using Microsoft.AspNetCore.Mvc;
using TestService;

namespace 异常筛选器.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly DemoService demoService;

        public TestController(DemoService demoService)
        {
            this.demoService = demoService;
        }

        [HttpGet]
        public ActionResult<string> GetMessage()
        {
            int num = demoService.GetNum(4, 0);
            return "返回数字是:" + num;
        }
    }
}
