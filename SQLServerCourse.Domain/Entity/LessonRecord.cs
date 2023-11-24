using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace SQLServerCourse.Domain.Entity
{
    public class LessonRecord
    {
        public long Id { get; set; }

        public byte LessonId { get; set; }

        public long UserId { get; set; }

        public User User { get; set; } 
        
        public Lesson Lesson { get; set;}

        public float Mark { get; set; }

        public DateTime DateOfReceiving { get; set; } = DateTime.Now;
    }
} 
