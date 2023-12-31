﻿using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.Domain.Extensions;
using SQLServerCourse.Domain.Responce;
using SQLServerCourse.Domain.ViewModels.PersonalProfile;
using SQLServerCourse.Domain.ViewModels.Review;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;

namespace SQLServerCourse.Controllers
{
    public class ReviewController : Controller
    {
        private const string defaultReferrer = "GetReviews";
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
        public async Task<IActionResult> CreateReview(CreateReviewViewModel review)
        {
            var response = await _reviewService.CreateReview(review, User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return RedirectToAction("GetReviews", "Review");
            }
            return PartialView("Error", $"{response.Description}");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteReview(long id, string referrer)
        {
            var response = await _reviewService.DeleteReview(id);
            if (response.StatusCode == Domain.Enum.StatusCode.OK && referrer == defaultReferrer)
            {
                return RedirectToAction("GetReviews", "Review");
            }
            else
            {
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    return RedirectToAction("GetUsers", "User");
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserReviews(long id)
        {
            var response = await _reviewService.GetUserReviews(id);

            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return View("Error", $"{response.Description}");
        }
    }
}
