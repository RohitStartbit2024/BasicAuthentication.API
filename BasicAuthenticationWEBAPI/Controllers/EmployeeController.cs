using BasicAuthenticationWEBAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BasicAuthenticationWEBAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        [BasicAuthentication]
        [HttpGet]
        public IActionResult GetEmployees()
        {
            string username = HttpContext.User.Identity.Name; 

            var empList = new EmployeeBL().GetEmployees();

            switch (username.ToLower())
            {
                case "maleuser":
                    var maleEmployees = empList.Where(e => e.Gender.ToLower() == "male").ToList();
                    return Ok(maleEmployees);
                case "femaleuser":
                    var femaleEmployees = empList.Where(e => e.Gender.ToLower() == "female").ToList();
                    return Ok(femaleEmployees); 
                default:
                    return BadRequest("Invalid user.");
            }
        }
    }
}
