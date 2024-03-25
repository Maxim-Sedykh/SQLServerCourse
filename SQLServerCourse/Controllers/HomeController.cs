using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.DAL.Contexts;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Service.Interfaces;
using System.Diagnostics;

namespace SQLServerCourse.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHomeService _homeService;
        private readonly CourseDbContext _db;
        private readonly FilmDbContext _dbFilm;

        public HomeController(IHomeService homeService, CourseDbContext db, FilmDbContext dbrr)
        {
            _homeService = homeService;
            _db = db;
            _dbFilm = dbrr;
        }

        public IActionResult Index() => View();

        public IActionResult Documentary() => PartialView();

        [HttpGet]
        public IActionResult GetCoursePlan()
        {
            var response = _homeService.GetCoursePlan();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data.ToList());
            }
            return View("Error", $"{response.Description}");
        }
    }
}