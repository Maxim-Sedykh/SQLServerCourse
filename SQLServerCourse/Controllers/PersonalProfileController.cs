using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using ServiceStack;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Service.Interfaces;
using System.Text.Json;

namespace SQLServerCourse.Controllers
{
    public class PersonalProfileController : Controller
    {
        private readonly IPersonalProfileService _personalProfileService;

        public PersonalProfileController(IPersonalProfileService personalProfileService)
        {
            _personalProfileService = personalProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPersonalProfile()
        {
            var response = await _personalProfileService.GetPersonalProfile(User.Identity.Name);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateInfo([FromBody] ProfileViewModel model)
        {
            //return View();
            if (ModelState.IsValid)
            {
                var response = await _personalProfileService.UpdateInfo(model);
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
            var response = await _personalProfileService.GetLessonRecords(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return PartialView("Error", $"{response.Description}");
        }

        [HttpGet]
        public IActionResult GetLessonList()
        {
            var response = _personalProfileService.GetLessonList();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return PartialView("Error", $"{response.Description}");
        }
    }
}
