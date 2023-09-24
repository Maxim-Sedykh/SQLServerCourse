using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetReviews()
        {
            var response = _reviewService.GetReviews();
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        [HttpPost]
        public IActionResult CreateReview()
        {
            return View();
        }
    }
}
