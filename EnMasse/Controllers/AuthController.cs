using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnMasse.Models;
using System.Linq;
using System.Text.RegularExpressions;

namespace EnMasse.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext db;

        public AuthController(AppDbContext context)
        {
            db = context;
        }

        // LOGIN FIRST
        public IActionResult Login()
        {
            // If already logged in → go to dashboard
            if (HttpContext.Session.GetString("UserID") != null)
            {
                return RedirectToAction("Index", "Customer");
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Customer");
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // REGISTER (only accessed manually)
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(Registration reg, string Password)
        {
            // 🔒 PASSWORD VALIDATION
            if (string.IsNullOrEmpty(Password) ||
                Password.Length < 8 ||
                !Regex.IsMatch(Password, @"[a-zA-Z]") ||
                !Regex.IsMatch(Password, @"[0-9]") ||
                !Regex.IsMatch(Password, @"[^a-zA-Z0-9]"))
            {
                ModelState.AddModelError("", "Password must be strong (8+ chars, letter, number, symbol).");
            }

            if (ModelState.IsValid)
            {
                string username = reg.CompanyName;

                if (db.Users.Any(u => u.Username == username))
                {
                    ModelState.AddModelError("", "Company already registered.");
                    return View(reg);
                }

                db.Registrations.Add(reg);
                db.SaveChanges();

                User user = new User
                {
                    Username = username,
                    Password = Password,
                    Role = "Customer",
                    RegistrationID = reg.RegistrationID
                };

                db.Users.Add(user);
                db.SaveChanges();

                // ✅ AUTO LOGIN AFTER REGISTER
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToAction("Index", "Customer");
            }

            return View(reg);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}