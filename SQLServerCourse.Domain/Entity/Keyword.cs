using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.Entity
{
    public class Keyword
    {
        public int Id { get; set; }

        public string Word { get; set; }

        public List<QueryWord> QueryWords { get; set; }
    }
}
