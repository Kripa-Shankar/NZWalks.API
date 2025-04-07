using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NZWalks.API.Controllers
{
    //https://localhost:portnumber/api/students
    [Route("api/[controller]")]
    [ApiController]
    // ControllerBase is a class that provides functionality for handling HTTP requests
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents()
        {
           string[] studentNames=new string[] { "Tom", "Jerry", "Mickey", "Minnie" };

           return Ok(studentNames);
        }
    }
}
