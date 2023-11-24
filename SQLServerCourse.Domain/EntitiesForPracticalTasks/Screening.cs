using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entitys_for_lesson
{
    public class Screening
    {
        public long Id { get; set; }

        public byte HallId { get; set; }

        public long FilmId { get; set; }

        public DateTime Time { get; set; }

        public Hall Hall { get; set; }

        public Film Film { get; set; }

        public List<Ticket> Tickets { get; set; }
    }
}
