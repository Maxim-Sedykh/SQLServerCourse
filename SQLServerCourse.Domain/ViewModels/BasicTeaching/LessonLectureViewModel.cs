using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Teaching
{
    public class LessonLectureViewModel
    {
        public int Id { get; set; }

        public string LessonName { get; set; }

        public HtmlString LessonMarkup { get; set; }
    }
}
