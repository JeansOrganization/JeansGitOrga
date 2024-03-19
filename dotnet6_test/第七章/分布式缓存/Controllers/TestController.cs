using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace 分布式缓存.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly IDistributedCache distributedCache;
        private readonly BookService bookService;
        private readonly ILogger<TestController> logger;

        public TestController(IDistributedCache distributedCache, BookService bookService, ILogger<TestController> logger)
        {
            this.distributedCache = distributedCache;
            this.bookService = bookService;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<Book> GetBookById(int id)
        {
            string key = "Book_" + id;

            var book = JsonSerializer.Deserialize<Book>(distributedCache.GetString(key));
            if(book == null)
            {
                book = bookService.GetBookById(id);
                distributedCache.SetString(key,JsonSerializer.Serialize(book));
            }

            if (book == null)
            {
                logger.LogInformation($"没找到ID为{id}的书");
                return NotFound($"没找到ID为{id}的书");
            }
            logger.LogInformation($"查找成功，ID为{id}的书名是{book.BookName}");
            return book;

        }
    }
}
