using Microsoft.AspNetCore.Mvc;

namespace WebAPI的技术及选择.Controllers
{
    [ApiController]
    [Route("[Controller]/[Action]")]
    public class TestController : Controller
    {
        /*[HttpGet]
        public ActionResult<Person> GetPerson(int id)
        {
            if(id <= 0)
            {
                return BadRequest("id必须是正数");
            }
            else if(id == 1)
            {
                return new Person(1,"Jean",19);
            }
            else if(id == 2)
            {
                return new Person(2,"Jack",54);
            }
            else
            {
                return NotFound("没找到");
            }
        }*/


        [HttpGet("{id}")]
        public ActionResult<Person> GetPerson(int id)
        {
            if (id <= 0)
            {
                return BadRequest(new ErrorInfo(-1, "id必须是正数"));
            }
            else if (id == 1)
            {
                return new Person(1, "Jean", 19);
            }
            else if (id == 2)
            {
                return new Person(2, "Jack", 54);
            }
            else
            {
                return NotFound(new ErrorInfo(-2, "没找到"));
            }
        }


    }
}
