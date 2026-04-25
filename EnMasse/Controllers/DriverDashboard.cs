using EnMasse.Models;
using EnMasse.Data;
using Microsoft.AspNetCore.Mvc;

namespace EnMasse.Controllers
{
    public class DriverDashboard : Controller
    {
        public IActionResult Login()
        {
            return View(); // Driver Login page
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            var driver = MockDriverData.Authenticate(model.DUsername, model.DPassword);

            if (driver != null)
            {
                return RedirectToAction("Dashboard");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(ForgotPasswordViewModel model)
        {
            var driver = MockDriverData.GetDriverByUsername(model.DUsername);

            if (driver == null)
            {
                ModelState.AddModelError("", "Username not found.");
                return View(model);
            }

            if (model.NewPassword != model.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                model.UsernameExists = true;
                return View(model);
            }

            MockDriverData.UpdatePassword(model.DUsername, model.NewPassword);

            ViewBag.Message = "Password updated successfully!";
            return View();
        }



        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Login");
        }
    }

}