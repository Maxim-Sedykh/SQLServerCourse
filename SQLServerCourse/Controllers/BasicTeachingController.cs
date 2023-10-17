using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using ServiceStack.Host;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces.TeachingInterfaces;

namespace SQLServerCourse.Controllers
{
    public class BasicTeachingController : Controller
    {
        private readonly IBasicTeachingService _basicTeachingService;

        public BasicTeachingController(IBasicTeachingService teachingService)
        {
            _basicTeachingService = teachingService;
        }

        [HttpGet]
        public IActionResult ReadLesson(int id) 
        {
            var response = _basicTeachingService.GetLecture(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpGet]
        public IActionResult PassLesson(int id)
        {
            var response = _basicTeachingService.GetQuestions(id); //Здесь заполняются 3 свойства у модели LessonPassViewModel

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> PassLesson(LessonPassViewModel model) //Предполагаемый возврат через post с той же моделью, заполненной уже 4-мя свойствами
        {
            var response = await _basicTeachingService.PassLesson(model, User.Identity.Name); //Дозаполнение последнего свойства
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
    }
}
