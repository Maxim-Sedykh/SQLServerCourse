using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace SQLServerCourse.Domain.Entitys_for_lesson
{
    public class Film
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<Screening> Screenings { get; set; }
    }
}
