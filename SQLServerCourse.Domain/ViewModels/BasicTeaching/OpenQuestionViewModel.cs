using SQLServerCourse.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Teaching
{
    public class OpenQuestionViewModel
    {
        public byte Number { get; set; }

        public string DisplayQuestion { get; set; }

        public List<string> Answers { get; set; }
    }
}
