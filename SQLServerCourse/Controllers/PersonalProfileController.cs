using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.Service.Interfaces;

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
    }
}
