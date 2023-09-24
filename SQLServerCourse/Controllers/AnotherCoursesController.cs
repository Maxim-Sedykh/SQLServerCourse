using Microsoft.AspNetCore.Mvc;

namespace SQLServerCourse.Controllers
{
    public class AnotherCoursesController : Controller
    {
        public IActionResult GetAnotherCourses() => View();
    }
}
