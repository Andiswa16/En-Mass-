using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnMasse.Models;
using EnMasse.Data;
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

        // ================================
        // 🔹 SIMPLE MOCK USERS (ALL HERE)
        // ================================

        private class MockUser
        {
            public int Id { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string Role { get; set; }
        }

        private static readonly List<MockUser> mockUsers = new List<MockUser>
        {
            // 👨‍✈️ Driver
            new MockUser { Id = 1, Username = "driver1", Password = "Driver123!", Role = "Driver" },

            // 👨‍💼 Manager
            new MockUser { Id = 2, Username = "manager", Password = "Manager123!", Role = "Manager" },

            // 🛠 Admin
            new MockUser { Id = 3, Username = "admin", Password = "Admin123!", Role = "Admin" }
        };

        // ================================
        // 🔁 REDIRECT BASED ON ROLE
        // ================================
        private IActionResult RedirectToDashboard()
        {
            var role = HttpContext.Session.GetString("Role");

            if (role == "Manager")
                return RedirectToAction("Dashboard", "Manager");

            if (role == "Driver")
                return RedirectToAction("Dashboard", "DriverDashboard");

            if (role == "Admin")
                return RedirectToAction("Dashboard", "Admin");

            return RedirectToAction("Index", "Customer");
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UserID") != null)
                return RedirectToDashboard();

            return RedirectToAction("Login");
        }

        // ================================
        // 🔐 LOGIN
        // ================================
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UserID") != null)
                return RedirectToDashboard();

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Please enter both username and password.";
                return View();
            }

            // 🔹 1. CHECK MOCK USERS (Driver, Manager, Admin)
            var mockUser = mockUsers.FirstOrDefault(u =>
                u.Username == username && u.Password == password);

            if (mockUser != null)
            {
                HttpContext.Session.SetString("UserID", mockUser.Id.ToString());
                HttpContext.Session.SetString("Role", mockUser.Role);
                HttpContext.Session.SetString("Username", mockUser.Username);
                return RedirectToDashboard();
            }

            // 🔹 2. CUSTOMER (DATABASE)
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

        // ================================
        // 📝 REGISTER (CUSTOMERS ONLY)
        // ================================
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("UserID") != null)
                return RedirectToDashboard();

            return View();
        }

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

                var user = new User
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

        // ================================
        // 🚪 LOGOUT
        // ================================
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}