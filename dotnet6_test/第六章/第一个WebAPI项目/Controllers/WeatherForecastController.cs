using Microsoft.AspNetCore.Mvc;

namespace 第一个WebAPI项目.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public async Task<object> SaveNote(SaveNoteRequest snq)
        {
            string path = snq.title + ".txt";
            await System.IO.File.WriteAllTextAsync(path,snq.content);
            return new { code = "0", message = "保存成功" };
        }

        private string AA()
        {
            return "";
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public string BB()
        {
            return "";
        }

        [HttpGet]
        public ActionResult<int> GetCountryCore(string countryName)
        {
            if(countryName == "中国")
            {
                return 11;
            }
            else if(countryName == "美国")
            {
                return 12;
            }
            else
            {
                return NotFound("没找到！！！");
            }
        }
    }
}
