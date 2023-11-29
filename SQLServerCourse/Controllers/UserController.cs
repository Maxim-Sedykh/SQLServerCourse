using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> GetUsers()
        {
            var response = await _userService.GetUsers();

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        public IActionResult GetUser() => PartialView();

        [HttpPost]
        public async Task<IActionResult> DeleteUser([FromBody] long id)
        {
            var response = await _userService.DeleteUser(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return Json(new { description = response.Description });
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        public IActionResult EditUser()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetRoles()
        {
            var types = _userService.GetRoles();
            return Json(types.Data);
        }
    }
}
