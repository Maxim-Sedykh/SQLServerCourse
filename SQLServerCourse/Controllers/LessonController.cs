using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Controllers
{
    public class LessonController : Controller
    {
        private readonly IUserService _userService;

        public LessonController(IUserService userService)
        {
            _userService = userService;
        }

        //// GET: LessonController
        //public async Task<IActionResult> Index(long id)
        //{
        //    var response = await _userService.GetUser(id);

        //    if (response.StatusCode == Domain.Enum.StatusCode.OK)
        //    {
        //        return View(response.Data);
        //    }
        //    return View("Error", $"{response.Description}");
        //}
    }
}
