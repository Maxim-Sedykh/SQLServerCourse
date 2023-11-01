using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.DAL;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Controllers
{
    public class AdminController : Controller
    {
        public AdminController()
        {
            
        }

        public IActionResult ChangeUserData()
        {
            return View();
        }


        public IActionResult ChangeLessonData()
        {
            return View();
        }
    }
}
