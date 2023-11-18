using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SQLServerCourse.Domain.ViewModels.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        public string UsersLogin { get; set; }

        public string ReviewText { get; set; }

        public DateTime ReviewDateTime { get; set; }
    }
}
