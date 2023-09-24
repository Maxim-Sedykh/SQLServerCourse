﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entitys_for_lesson
{
    public class HallRow
    {
        public int Id { get; set; }

        public int HallId { get; set; }

        public short Number { get; set; }

        public short Capacity { get; set; }

        public Hall Hall { get; set; }
    }
}