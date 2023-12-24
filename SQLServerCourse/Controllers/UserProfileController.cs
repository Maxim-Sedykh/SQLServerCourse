using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using ServiceStack;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;
using System.Text.Json;

namespace SQLServerCourse.Controllers
{
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _profileService;

        public UserProfileController(IUserProfileService personalProfileService)
        {
            _profileService = personalProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserProfile(string userLogin)
        {
            var response = await _profileService.GetUserProfile(userLogin);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInfo([FromBody] UserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _profileService.UpdateInfo(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return Json(new { description = response.Description });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonRecords(long id)
        {
            var response = await _profileService.GetLessonRecords(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return PartialView("Error", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonList()
        {
            var response = await _profileService.GetLessonList(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return PartialView("Error", $"{response.Description}");
        }
    }
}
