using Microsoft.AspNetCore.Mvc;
using MVC项目.Models;

namespace MVC项目.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View(new Person("Jean",true,DateTime.Now.AddDays(-100)));
        }
    }
}
