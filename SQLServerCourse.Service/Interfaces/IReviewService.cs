using ServiceStack;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Domain.ViewModels.User;
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

        Task<IBaseResponse<bool>> CreateReview(CreateReviewViewModel model, string userLogin);

        Task<IBaseResponse<Review>> DeleteReview(long id);
        
        Task<IBaseResponse<List<ReviewViewModel>>> GetUserReviews(long id);
    }
}
