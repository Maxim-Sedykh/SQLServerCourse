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
        private readonly IBaseRepository<UserProfile> _userProfileRepository;

        public ReviewService(IBaseRepository<Review> reviewRepository, IBaseRepository<UserProfile> userProfileRepository)
        {
            _reviewRepository = reviewRepository;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<IBaseResponse<bool>> CreateReview(CreateReviewViewModel model, string userName)
        {
            try
            {
                var profile = await _userProfileRepository.GetAll().FirstOrDefaultAsync(x => x.User.Login == userName);
                if (profile == null)
                {
                    return new BaseResponse<bool>
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.UserNotFound
                    };
                }

                var review = new Review()
                {
                    UserId = profile.Id,
                    ReviewText = model.ReviewText,
                    ReviewTime = DateTime.Now,
                };

                profile.IsReviewLeft = true;

                await _reviewRepository.Create(review);
                await _userProfileRepository.Update(profile);


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
                        UserId = x.UserId,
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
