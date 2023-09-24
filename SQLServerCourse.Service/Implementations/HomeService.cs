using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SQLServerCourse.Service.Implementations
{
    public class HomeService: IHomeService
    {
        private readonly IBaseRepository<Lesson> _lessonRepository;

        public HomeService(IBaseRepository<Lesson> lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public BaseResponse<IEnumerable<string>> GetCoursePlan()
        {
            try
            {
                var lessons = from lesson in _lessonRepository.GetAll()
                              select lesson.Name;

                if (lessons is null)
                {
                    return new BaseResponse<IEnumerable<string>>()
                    {
                        Description = "Уроки не найдены",
                        StatusCode = StatusCode.LessonNotFound
                    };
                }

                return new BaseResponse<IEnumerable<string>>()
                {
                    Data = lessons.ToList(),
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<string>>()
                {
                    Description = $"Внутренняя ошибка : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}
