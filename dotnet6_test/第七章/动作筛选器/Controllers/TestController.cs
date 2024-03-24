using Microsoft.AspNetCore.Mvc;

namespace 动作筛选器.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetData(string param)
        {
            //int i = 0;
            //int y = 10/ i;
            Console.WriteLine("执行GetData");
            return param + ":Jean";
        }
    }
}
