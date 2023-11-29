using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.FinalResult
{
    public class ResultViewModel
    {
        public string Login { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public float CurrentGrade { get; set; }

        public string UserAnalys { get; set; }
    }
}
