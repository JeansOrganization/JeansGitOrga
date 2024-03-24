using BookEFCore;
using CarEFCore;
using Microsoft.AspNetCore.Mvc;

namespace 自动启用事务的筛选器.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly BookDbContext bookContext;
        private readonly CarDbContext carContext;

        public TestController(BookDbContext bookContext, CarDbContext carContext)
        {
            this.bookContext = bookContext;
            this.carContext = carContext;
        }

        [NotTransactional]
        [HttpGet]
        public ActionResult<string> SaveData()
        {
            bookContext.Books.Add(new Book() { Name = "好书", Price = 122 });
            bookContext.SaveChanges();
            carContext.Cars.Add(new Car() { Name = "玛莎拉蒂玛莎拉蒂玛莎拉蒂", Price = 300 });
            carContext.SaveChanges();
            return "保存成功";
        }
    }
}
