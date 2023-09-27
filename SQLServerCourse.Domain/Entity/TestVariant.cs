using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class TestVariant
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }

        public Question Question { get; set; }

        public int VariantNumber { get; set; }

        public string Content { get; set; }

        public bool IsRight { get; set; }
    }
}
