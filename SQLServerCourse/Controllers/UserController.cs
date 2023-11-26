using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SQLServerCourse.Controllers
{
    public class UserController : Controller
    {
        public UserController()
        {
                
        }

        public IActionResult GetUsers()
        {
            return View();
        }

        public IActionResult CreateUser()
        {
            return View();
        }

        public IActionResult DeleteUser()
        {
            return View();
        }

        public IActionResult EditUser()
        {
            return View();
        }
    }
}
