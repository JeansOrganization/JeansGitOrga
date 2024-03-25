using Microsoft.AspNetCore.Mvc;

namespace 自定义特性的运用.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> GetData()
        {
            User user1 = new User();
            user1.Id = 222203;
            user1.Name = "Jean";
            if (!user1.IsValid(out string msg))
            {
                Console.WriteLine("user1数据有误:" + msg);
            }
            User user2 = new User();
            user2.Id = 301;
            user2.Name = "Ting";
            if (!user2.IsValid(out msg))
            {
                Console.WriteLine("user2数据有误:" + msg);
            }

            return "11";
        }
    }
}
