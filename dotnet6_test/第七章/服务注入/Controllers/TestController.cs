using Hispital;
using Microsoft.AspNetCore.Mvc;
using School;

namespace 服务注入.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        public readonly HispitalService hispitalService;
        public readonly SchoolService schoolService;
        public TestController(HispitalService hispitalService,SchoolService schoolService)
        {
            this.hispitalService = hispitalService; 
            this.schoolService = schoolService;
        }

        [HttpGet]
        public ActionResult<int> GetTenFold(int x)
        {
            return x * 10;
        }

        [HttpGet]
        public ActionResult<int> GetFilesCount([FromServices]MyService myService, int x)
        {
            int result = myService.GetFilesCount();
            return result;
        }

        [HttpGet]
        public ActionResult<string> GetName([FromServices] MyService myService, int x)
        {
            var nameArr = myService.GetNames();
            return string.Join(", ", nameArr) + x;
        }

        [HttpGet]
        public ActionResult<string> GetPresidentName()
        {
            return hispitalService.GetPresidentName();
        }

        [HttpGet]
        public ActionResult<int> GetStudentCount()
        {
            return schoolService.GetStudentCount();
        }
    }
}
