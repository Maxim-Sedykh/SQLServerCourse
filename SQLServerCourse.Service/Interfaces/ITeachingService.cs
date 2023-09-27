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
    public interface ITeachingService
    {
        public IBaseResponse<LessonLectureViewModel> GetLecture(int lessonId);

        public Task<IBaseResponse<LessonPassViewModel>> GetQuestions(int lessonId);

        //public Task<IBaseResponse<ResultViewModel>> PassExam(LessonPassViewModel model, string userName);

        //public Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userName);
    }
}
