using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class FrequentRemark
    {
        public int Id { get; set; }

        public string Keyword { get; set; }

        public string Remark { get; set; }

        public List<PracticalTask> PracticalTask { get; set; }
    }
}
