using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.DAL;
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
        private readonly ApplicationDbContext _db;

        public HomeController(IHomeService homeService, ApplicationDbContext db)
        {
            _homeService = homeService;
            _db = db;
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
            return PartialView("Error", $"{response.Description}");
        }
    }
}