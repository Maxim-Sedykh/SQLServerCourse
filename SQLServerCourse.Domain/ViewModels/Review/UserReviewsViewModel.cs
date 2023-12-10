using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Domain.ViewModels.Review
{
    public class UserReviewsViewModel
    {
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Поле \"ID пользователя\" должно быть числом")]
        public long UserId { get; set; }
    }
}
