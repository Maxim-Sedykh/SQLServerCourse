using Microsoft.EntityFrameworkCore;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace SQLServerCourse.Service.Implementations
{
    public class ReviewService : IReviewService
    {
        private readonly IBaseRepository<Review> _reviewRepository;
        
        public ReviewService(IBaseRepository<Review> reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<IBaseResponse<bool>> CreateReview(CreateReviewViewModel model, int userId)
        {
            try
            {
                var review = new Review()
                {
                    UserId = userId,
                    ReviewText = model.ReviewText,
                };

                await _reviewRepository.Create(review);

                return new BaseResponse<bool>()
                {
                    StatusCode = StatusCode.OK,
                    Description = "Отзыв успешно создан."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }

        public IBaseResponse<IEnumerable<ReviewViewModel>> GetReviews()
        {
            try
            {
                var reviews = _reviewRepository.GetAll()
                    .Include(x => x.User)
                    .Select(x => new ReviewViewModel()
                    {
                        UsersLogin = x.User.Login,
                        ReviewText = x.ReviewText,
                    })
                    .ToList();

                return new BaseResponse<IEnumerable<ReviewViewModel>>()
                {
                    Data = reviews,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ReviewViewModel>>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
    }
}
