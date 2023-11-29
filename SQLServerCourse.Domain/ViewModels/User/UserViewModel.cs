using SQLServerCourse.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.User
{
    public class UserViewModel
    {
        public long Id { get; set; }

        public string Login { get; set; }

        public Role Role { get; set; }

        public bool IsExamCompleted { get; set; }

        public bool IsReviewLeft { get; set; }

        public bool IsEditAble { get; set; }
    }
}
