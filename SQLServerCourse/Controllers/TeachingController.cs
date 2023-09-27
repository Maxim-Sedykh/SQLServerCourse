using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Host;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Controllers
{
    public class TeachingController : Controller
    {
        private readonly ITeachingService _teachingService;

        public TeachingController(ITeachingService teachingService)
        {
            _teachingService = teachingService;
        }

        [HttpGet]
        public IActionResult ReadLesson(int id) 
        {
            var response = _teachingService.GetLecture(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpGet]
        public async Task<IActionResult> PassLesson(int id)
        {
            var response = await _teachingService.GetQuestions(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        //[HttpPost]
        //public async Task<IActionResult> PassLesson(LessonPassViewModel model)
        //{
        //    var response = await _teachingService.PassLesson(model, User.Identity.Name);
        //    if (response.StatusCode == Domain.Enum.StatusCode.OK)
        //    {
        //        return View(response.Data);
        //    }
        //    return View("Error", $"{response.Description}");
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetResult(LessonPassViewModel model)
        //{
        //    var response = await _teachingService.PassExam(model, User.Identity.Name);
        //    if (response.StatusCode == Domain.Enum.StatusCode.OK)
        //    {
        //        return View(response.Data);
        //    }
        //    return View("Error", $"{response.Description}");
        //}

        
    }
}
