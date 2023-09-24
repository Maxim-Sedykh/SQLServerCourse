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
        public int Id { get; set; }

        public string NameOfFilm { get; set; }

        public string Description { get; set; }

        public List<Screening> Screening { get; set; }
    }
}
