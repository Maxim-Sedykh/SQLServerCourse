using Microsoft.AspNetCore.Mvc;

namespace SQLServerCourse.Controllers
{
    public class FinalExamController : Controller
    {
        [HttpGet]
        public IActionResult PassExam()
        {
            return View();
        }

        public IActionResult GetLearningResult()
        {
            return View();
        }

        public IActionResult GetAnalys()
        {
            return PartialView();
        }

        public IActionResult GetExamResult()
        {
            return PartialView();
        }
    }
}
