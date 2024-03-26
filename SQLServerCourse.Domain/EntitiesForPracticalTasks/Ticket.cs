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
        public long Id { get; set; }

        public int Row { get; set; }

        public int Seat { get; set; }

        public decimal Cost { get; set; }

        public long ScreeningId { get; set; }

        public Screening Screening { get; set; }
    }
}   
