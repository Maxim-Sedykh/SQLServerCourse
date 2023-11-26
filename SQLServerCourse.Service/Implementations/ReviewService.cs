using Microsoft.EntityFrameworkCore;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Domain.Enum;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
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
        private readonly IBaseRepository<User> _userRepository;

        public ReviewService(IBaseRepository<Review> reviewRepository, IBaseRepository<User> userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<IBaseResponse<bool>> CreateReview(CreateReviewViewModel model, string userName)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == userName);
                if (user == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var review = new Review()
                {
                    UserId = user.Id,
                    ReviewText = model.ReviewText,
                    ReviewTime = DateTime.Now,
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

        public async Task<IBaseResponse<Review>> DeleteReview(long id)
        {
            try
            {
                var review = await _reviewRepository.GetAll()
                    .FirstOrDefaultAsync(x => x.Id == id);
                if (review == null)
                {
                    return new BaseResponse<Review>()
                    {
                        StatusCode = StatusCode.UserNotFound,
                        Description = "Отзыв не найден"
                    };
                }

                await _reviewRepository.Delete(review);

                return new BaseResponse<Review>()
                {
                    Data = review,
                    Description = "Отзыв удалён!",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Review>()
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = $"Внутренняя ошибка: {ex.Message}"
                };
            }
        }

        public async Task<IBaseResponse<List<ReviewViewModel>>> GetReviews()
        {
            try
            {
                var reviews = await _reviewRepository.GetAll()
                    .Include(x => x.User)
                    .Select(x => new ReviewViewModel()
                    {
                        Id = x.Id,
                        Login = x.User.Login,
                        Text = x.ReviewText,
                        ReviewDateTime = x.ReviewTime,
                    }).ToListAsync();

                return new BaseResponse<List<ReviewViewModel>>()
                {
                    Data = reviews,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ReviewViewModel>>()
                {
                    Description = $"Внутренняя ошибка: {ex.Message}",
                    StatusCode = StatusCode.InternalServerError,
                };
            }
        }
    }
}
