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
        public long Id { get; set; }

        public string Login { get; set; }

        public string Text { get; set; }

        public DateTime ReviewDateTime { get; set; }
    }
}
