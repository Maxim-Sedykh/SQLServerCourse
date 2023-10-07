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

namespace SQLServerCourse.Domain.Entity
{
    public class Review
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public string ReviewText { get; set; }

        public DateTime ReviewTime { get; set; }
    }
}
