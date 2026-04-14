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

        // Helper Method: Routes users to the correct domain based on their role
        private IActionResult RedirectToDashboard()
        {
            var role = HttpContext.Session.GetString("Role");
            if (role == "Manager")
            {
                return RedirectToAction("Dashboard", "Manager");
            }
            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserID") != null)
                return RedirectToDashboard();

            return RedirectToAction("Login");
        }

        // LOGIN GET
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserID") != null)
                return RedirectToDashboard();

            return View();
        }

        // LOGIN POST
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both username and password.";
                return View();
            }

            // The Manager Backdoor
            if (username == "Manager" && password == "Logistics2026!")
            {
                HttpContext.Session.SetString("UserID", "9999");
                HttpContext.Session.SetString("Role", "Manager");
                HttpContext.Session.SetString("Username", "Jane Doe");
                return RedirectToAction("Dashboard", "Manager");
            }

            // Standard Customer Login
            var user = db.Users.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                return RedirectToDashboard();
            }

            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // REGISTER GET
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserID") != null)
                return RedirectToDashboard();

            return View();
        }

        // REGISTER POST
        [HttpPost]
        public IActionResult Register(Registration reg, string Password)
        {
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

                HttpContext.Session.SetString("UserID", user.UserID.ToString());
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.Username);

                TempData["Success"] = "Registration successful! Welcome, " + username + ".";
                return RedirectToDashboard();
            }

            return View(reg);
        }

        // LOGOUT
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}