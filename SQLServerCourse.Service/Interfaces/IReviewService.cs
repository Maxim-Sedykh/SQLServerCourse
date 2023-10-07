using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Interfaces
{
    public interface IReviewService
    {
        Task<IBaseResponse<List<ReviewViewModel>>> GetReviews();

        Task<IBaseResponse<bool>> CreateReview(CreateReviewViewModel model, string userName);
    }
}
