using BookEFCore;
using CarEFCore;
using Microsoft.AspNetCore.Mvc;

namespace EFCore测试用WebAPI项目.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly BookDbContext dbContext;
        private readonly CarDbContext carDbContext;

        public TestController(BookDbContext dbContext, CarDbContext carDbContext)
        {
            this.dbContext = dbContext;
            this.carDbContext = carDbContext;
        }

        [HttpGet]
        public ActionResult<List<object>> GetAllData()
        {
            List<Book> bookList = dbContext.Books.ToList();
            List<Car> carList = carDbContext.Cars.ToList();
            var list = new List<object>() { bookList, carList };
            return list;
        }
    }
}
