using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entitys_for_lesson
{
    public class Ticket
    {
        public int Id { get; set; }

        public int ScreeningId { get; set; }

        public short Row { get; set; }

        public short Seat { get; set; }

        public int Cost { get; set; }

        public Screening Screening { get; set; }
    }
}   
