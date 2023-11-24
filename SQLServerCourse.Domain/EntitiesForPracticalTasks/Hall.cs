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
        public byte Id { get; set; }

        public string Name { get; set; }

        public List<HallRow> HallRows { get; set; }

        public List<Screening> Screenings { get; set; }
    }
}
