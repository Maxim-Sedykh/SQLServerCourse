using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entitys_for_lesson
{
    public class Hall
    {
        public int Id { get; set; }

        public string HallName { get; set; }

        public List<HallRow> HallRow { get; set; }

        public List<Screening> Screening { get; set; }
    }
}
