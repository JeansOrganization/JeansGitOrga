using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace 内存缓存.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class TestController : ControllerBase
    {
        private readonly BookService bookService;
        private readonly IMemoryCache memoryCache;
        private readonly IMemoryCacheHelper memoryCacheHelper;
        private readonly ILogger<TestController> logger;

        public TestController(BookService bookService,IMemoryCache memoryCache, IMemoryCacheHelper memoryCacheHelper, ILogger<TestController> logger)
        {
            this.bookService = bookService;
            this.memoryCache = memoryCache;
            this.memoryCacheHelper = memoryCacheHelper;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<Book?>> GetBookById11(int id)
        {
            string key = "book-" + id;
            Book? book = null;
            logger.LogInformation($"开始查找缓存中id为{id}的书");
            if (!memoryCache.TryGetValue(key,out book))
            {
                logger.LogInformation($"缓存中不存在，开始查找数据库中id为{id}的书");
                book = await bookService.GetBookByIdAsync(id);
                memoryCache.Set(key,book, new MemoryCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20),
                    SlidingExpiration = TimeSpan.FromSeconds(10)
                });
            }

            if (book == null)
            {
                logger.LogInformation($"没找到ID为{id}的书");
                return NotFound($"没找到ID为{id}的书");
            }
            logger.LogInformation($"查找成功，ID为{id}的书名是{book.BookName}");
            return book;
        }

        [HttpGet]
        public async Task<ActionResult<Book?>> GetBookById(int id)
        {
            string key = "book-" + id; 
            logger.LogInformation($"开始查找缓存中id为{id}的书");
            var book = await memoryCache.GetOrCreateAsync(key, async e =>
            {
                logger.LogInformation($"XXXXXXXXXX缓存中不存在，开始查找数据库中id为{id}的书");
                //绝对过期时间
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(Random.Shared.Next(30, 40));
                //滑动过期时间
                e.SlidingExpiration = TimeSpan.FromSeconds(Random.Shared.Next(5,10));
                return await bookService.GetBookByIdAsync(id);
            });
            if (book == null)
            {
                logger.LogInformation($"没找到ID为{id}的书");
                return NotFound($"没找到ID为{id}的书");
            }
            logger.LogInformation($"查找成功，ID为{id}的书名是{book.BookName}");
            return book;
        }

        [HttpGet]
        public ActionResult<List<Book>> GetAllBooks()
        {
            string key = "AllBooks";
            logger.LogInformation($"开始查找缓存所有的书");
            var books = memoryCacheHelper.GetOrCreate(key, e =>
            {
                logger.LogInformation($"XXXXXXXXXX缓存中不存在，开始查找数据库中所有的书");
                e.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10);
                return bookService.GetAllBooks().ToList();
            });
            if (books == null || books.Count() == 0)
            {
                logger.LogInformation($"没找到所有的书");
                return NotFound($"没找到所有的书");
            }
            logger.LogInformation($"查找成功");
            return books.ToList();
        }


    }
}
