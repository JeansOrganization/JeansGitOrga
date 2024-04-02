using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace 用MediatR实现领域事件.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class DemoController : ControllerBase
    {
        private readonly ILogger<DemoController> _logger;
        private readonly IMediator mediator;

        public DemoController(ILogger<DemoController> logger, IMediator mediator)
        {
            _logger = logger;
            this.mediator = mediator;
        }

        [HttpGet]
        public ActionResult Get()
        {
            TestNotification notification = new TestNotification("JEAN");//触发所有NotificationHandler
            TestRequest request = new TestRequest("JEAN");//只会触发一个RequestHandler
            mediator.Publish(notification);
            mediator.Send(request);
            return Ok(notification);
        }
    }
}
