﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class QueryWord
    {
        public long Id { get; set; }

        public int Number { get; set; }

        public int KeywordId { get; set; }

        public Keyword Keyword { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }
    }
}
