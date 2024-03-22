using EFCore;
using Microsoft.AspNetCore.Mvc;

namespace EFCore测试用WebAPI项目.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly MyDbContext dbContext;

        public TestController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetBooks()
        {
            return dbContext.Books.ToList();
        }
    }
}
