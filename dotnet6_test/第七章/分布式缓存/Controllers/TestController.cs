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
        private readonly IDistributedCacheHelper cacheHelper;
        private readonly ILogger<TestController> logger;

        public TestController(IDistributedCache distributedCache, BookService bookService, IDistributedCacheHelper cacheHelper, ILogger<TestController> logger)
        {
            this.distributedCache = distributedCache;
            this.bookService = bookService;
            this.cacheHelper = cacheHelper;
            this.logger = logger;
        }

        [HttpGet]
        public ActionResult<Book> GetBookById(int id)
        {
            string key = "Book_" + id;
            Book? book = cacheHelper.GetOrCreate(key, e =>
            {
                logger.LogInformation($"XXXXXXXXXX\r\n缓存中不存在，开始查找数据库中id为{id}的书\r\nXXXXXXXXXX");
                return bookService.GetBookById(id);
            }, 30);

            if (book == null)
            {
                logger.LogInformation($"没找到ID为{id}的书");
                return NotFound($"没找到ID为{id}的书");
            }
            logger.LogInformation($"查找成功，ID为{id}的书名是{book.BookName}");
            return book;

        }

        [HttpGet("id")]
        public async Task<ActionResult<Book>> GetBookByIdAsync(int id)
        {
            string key = "Book_" + id;
            Book? book = await cacheHelper.GetOrCreateAsync(key, e =>
            {
                logger.LogInformation($"XXXXXXXXXX\r\n缓存中不存在，开始查找数据库中id为{id}的书\r\nXXXXXXXXXX");
                return bookService.GetBookByIdAsync(id);
            }, 30);

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
