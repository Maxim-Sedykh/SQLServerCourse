using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Lesson;
using SQLServerCourse.Domain.ViewModels.Teaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces.TeachingInterfaces
{
    public interface IBasicTeachingService
    {
        public IBaseResponse<LessonLectureViewModel> GetLecture(int lessonId);

        public IBaseResponse<LessonPassViewModel> GetQuestions(int lessonId);

        public Task<IBaseResponse<LessonPassViewModel>> PassLesson(LessonPassViewModel model, string userName);
    }
}
