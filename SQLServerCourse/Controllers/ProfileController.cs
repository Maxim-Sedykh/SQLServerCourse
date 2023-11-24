using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using ServiceStack;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Service.Interfaces;
using System.Text.Json;

namespace SQLServerCourse.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService personalProfileService)
        {
            _profileService = personalProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var response = await _profileService.GetProfile(User.Identity.Name);

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
        public async Task<IActionResult> GetLessonRecords()
        {
            var response = await _profileService.GetLessonRecords(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return PartialView("Error", $"{response.Description}");
        }

        [HttpGet]
        public IActionResult GetLessonList()
        {
            var response = _profileService.GetLessonList();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return PartialView("Error", $"{response.Description}");
        }
    }
}
