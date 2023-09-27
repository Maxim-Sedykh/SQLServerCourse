using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Teaching
{
    public class TestQuestionViewModel
    {
        public byte Number { get; set; }

        public string DisplayQuestion { get; set; }

        public List<TestVariant> VariantsOfAnswer { get; set; }

        public string? RightPageAnswer { get; set; }

        public string InnerAnswer { get; set; }
    }
}
