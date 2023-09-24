using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Teaching
{
    public class QuestionViewModel
    {
        public string Content { get; set; }

        public List<PageAnswer>? VariantsOfAnswer { get; set; }

        public TaskType QuestionType { get; set; }
    }
}
