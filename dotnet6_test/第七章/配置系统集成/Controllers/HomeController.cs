using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace 配置系统集成.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IOptionsSnapshot<SmtpOptions> smtpOptions;
        private readonly IDistributedCacheHelper disCacheHelper;
        private readonly BookService bookService;
        private readonly ILogger<HomeController> logger;
        public HomeController(IConfiguration _configuration, IWebHostEnvironment webHostEnvironment,
            IOptionsSnapshot<SmtpOptions> smtpOptions, IDistributedCacheHelper disCacheHelper, BookService bookService, 
            ILogger<HomeController> logger)
        {
            this._configuration = _configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.smtpOptions = smtpOptions;
            this.disCacheHelper = disCacheHelper;
            this.bookService = bookService;
            this.logger = logger;

        }

        [HttpGet]
        public ActionResult<string> GetString()
        {
            string logLevel_Default = _configuration.GetSection("Logging:LogLevel:Default").Value;

            string logLevel_Microsoft_AspNetCore = _configuration["Logging:LogLevel:Microsoft.AspNetCore"];

            Proxy proxy = new Proxy();
            _configuration.GetSection("Proxy").Bind(proxy);

            string default1 = _configuration.GetValue<string>("Logging:LogLevel:Default");

            string? ASPNETCORE_ENVIRONMENT = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string webHostEnvironment_EnvironmentName = webHostEnvironment.EnvironmentName;

            if (webHostEnvironment.IsEnvironment("Development"))
            {
                if (webHostEnvironment.IsDevelopment())
                {
                    webHostEnvironment_EnvironmentName = webHostEnvironment.EnvironmentName;
                }
            }

            return $"logLevel_Default:{logLevel_Default}\r\n" +
                $"logLevel_Microsoft_AspNetCore:{logLevel_Microsoft_AspNetCore}\r\n" +
                $"default1:{default1}\r\n" +
                $"ASPNETCORE_ENVIRONMENT:{ASPNETCORE_ENVIRONMENT}\r\n" +
                $"webHostEnvironment_EnvironmentName:{webHostEnvironment_EnvironmentName}";
        }



        [HttpGet]
        public ActionResult<Book> GetBookById(int id)
        {
            string key = "Book_" + id;
            Book? book = disCacheHelper.GetOrCreate(key, e =>
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
            Console.WriteLine(smtpOptions.Value);
            return book;

        }
    }
}
