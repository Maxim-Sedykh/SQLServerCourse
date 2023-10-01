using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Teaching
{
    public class OpenQuestionViewModel
    {
        public int Number { get; set; }

        public string DisplayQuestion { get; set; }

        public List<string> InnerAnswers { get; set; }

        public string UserAnswer { get; set; }

        public bool AnswerCorrectness { get; set; }
    }
}
