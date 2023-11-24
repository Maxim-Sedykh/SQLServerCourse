using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using SQLServerCourse.Domain.Enum;

namespace SQLServerCourse.Domain.Entity
{
    public class User
    {
        public long Id { get; set; }

        public string Login { get; set; }                                                                                                                                                                                            
            
        public string Password { get; set; }

        public Role Role { get; set; }

        public List<Review> Reviews { get; set; }

        public List<LessonRecord> LessonRecords { get; set; }

        public UserProfile UserProfile { get; set; }
    }
}
