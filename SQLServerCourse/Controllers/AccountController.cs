using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SQLServerCourse.Domain.ViewModels.Account;
using System.Security.Claims;
using SQLServerCourse.Service.Interfaces;
using SQLServerCourse.DAL.Repositories;

namespace SQLServerCourse.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Register(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", response.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.Login(model);
                if (response.StatusCode == Domain.Enum.StatusCode.OK)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(response.Data));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", response.Description);
                } 
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserPassword([FromBody] ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _accountService.ChangePassword(model, User.Identity.Name);
                if (response.StatusCode == Domain.Enum.StatusCode.OK && response.Data)
                {
                    return Json(new { description = response.Description });
                }
            }
            var modelError = ModelState.Values.SelectMany(v => v.Errors);

            return StatusCode(StatusCodes.Status500InternalServerError, new { modelError.FirstOrDefault().ErrorMessage });
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
