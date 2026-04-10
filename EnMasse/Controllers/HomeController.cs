using System.Diagnostics;
using EnMasse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace EnMasse.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ✅ ALWAYS go to LOGIN first if not logged in
            if (HttpContext.Session.GetString("UserID") == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // ✅ If already logged in → dashboard
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}