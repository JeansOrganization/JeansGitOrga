using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Nlog.WebApiDemo.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<DemoController> logger;

        public DemoController(ILogger<DemoController> logger, IHttpContextAccessor httpContextAccessor)
        {
            this.logger = logger;
            this.httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public IActionResult Get(LoginRequest req)
        {
            //logger.LogError(JsonConvert.SerializeObject(req));
            int i = 0;
            int num = 10 / i;
            return Ok();
        }
    }
}
