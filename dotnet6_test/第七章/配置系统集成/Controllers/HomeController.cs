using Microsoft.AspNetCore.Mvc;

namespace 配置系统集成.Controllers
{
    [ApiController]
    [Route("[controller]/[Action]")]
    public class HomeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IWebHost webHost;
        public HomeController(IConfiguration _configuration, IWebHostEnvironment webHostEnvironment, IWebHost webHost)
        {
            this._configuration = _configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.webHost = webHost;
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
        public async Task HostStop()
        {
            await webHost.StopAsync();
        }
    }
}
