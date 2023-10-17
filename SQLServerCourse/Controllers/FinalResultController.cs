using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ServiceStack.Script;
using SQLServerCourse.DAL.Interfaces;
using SQLServerCourse.DAL.Repositories;
using SQLServerCourse.Domain.Entity;
using SQLServerCourse.Service.Implementations;
using SQLServerCourse.Service.Interfaces;
using SQLServerCourse.Service.Interfaces.TeachingInterfaces;

namespace SQLServerCourse.Controllers
{
    public class FinalResultController : Controller
    {
        private readonly IFinalResultService _finalResultService;

        public FinalResultController(IFinalResultService finalResultService)
        {
            _finalResultService = finalResultService;
        }
        
        public async Task<IActionResult> GetFinalResult()
        {
            var response = await _finalResultService.GetResultModel(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return View(response.Data);
            }
            return View("Error", $"{response.Description}");
        }

        public IActionResult GetUserAnalys()
        {
            var response = _finalResultService.GetUserAnalys(User.Identity.Name);
            if (response.StatusCode == Domain.Enum.StatusCode.OK)
            {
                return PartialView(response.Data);
            }
            return View("Error", $"{response.Description}");
        }


        //[HttpGet]
        //public IActionResult GetUserAnalys()
        //{
        //    var response = _finalResultService.GetUserAnalys(User.Identity.Name);
        //    if (response.StatusCode == Domain.Enum.StatusCode.OK)
        //    {
        //        return PartialView(response.Data);
        //    }
        //    return View("Error", $"{response.Description}");
        //}
    }
}



