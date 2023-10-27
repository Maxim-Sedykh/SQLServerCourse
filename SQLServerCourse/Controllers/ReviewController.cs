using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Controllers
{
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet]
        public async Task<IActionResult> GetReviews()
        {
            var response = await _reviewService.GetReviews();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpGet]
        public IActionResult CreateReview() => PartialView();

        [HttpPost]
        public async Task<IActionResult> CreateReview(CreateReviewViewModel review, string userName)
        {
            var response = await _reviewService.CreateReview(review, User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetReviews", "Review");
            }
            return PartialView("Error", $"{response.Description}");
        }
    }
}
