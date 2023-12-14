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
    public class LessonController : Controller
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService teachingService)
        {
            _lessonService = teachingService;
        }

        [HttpGet]
        public IActionResult ReadLesson(byte id) 
        {
            var response = _lessonService.GetLecture(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpGet]
        public IActionResult PassLesson(byte id)
        {
            var response = _lessonService.GetQuestions(id); //Здесь заполняются 3 свойства у модели LessonPassViewModel

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpPost]
        public async Task<IActionResult> PassLesson(LessonPassViewModel model) //Предполагаемый возврат через post с той же моделью, заполненной уже 4-мя свойствами
        {
            var response = await _lessonService.PassLesson(model, User.Identity.Name); //Дозаполнение последнего свойства
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
    }
}
