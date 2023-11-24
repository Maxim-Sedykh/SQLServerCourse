using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Teaching
{
    public class QuestionViewModel
    {
        public byte Number { get; set; }

        public string DisplayQuestion { get; set; }

        public TaskType QuestionType { get; set; }

        public List<TestVariant> VariantsOfAnswer { get; set; }

        public string RightPageAnswer { get; set; }

        public List<string> InnerAnswers { get; set; }

        public string UserAnswer { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
