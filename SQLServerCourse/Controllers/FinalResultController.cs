using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack.Script;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;

namespace SQLServerCourse.Controllers
{
    public class FinalResultController : Controller
    {

        public FinalResultController()
        {

        }

        public IActionResult GetAnalys()
        {

            return PartialView();
        }
    }
}



