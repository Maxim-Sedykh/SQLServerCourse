using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface ILessonService
    {
        public IBaseResponse<LessonLectureViewModel> GetLecture(byte lessonId);

        public IBaseResponse<LessonPassViewModel> GetQuestions(byte lessonId);

        public Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userLogin);

        public Task<IBaseResponse<LessonLectureViewModel>> SaveLectureMarkup(LessonContentViewModel model);
    }
}
