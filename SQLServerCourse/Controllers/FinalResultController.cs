using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack.Script;

namespace SQLServerCourse.Controllers
{
    public class FinalResultController : Controller
    {
        public IActionResult GetLearningResult()
        {
            return View();
        }

        public IActionResult GetAnalys()
        {
            return PartialView();
        }
    }
}



