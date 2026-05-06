using EnMasse.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EnMasse.Controllers
{
    public class DriverDashboardController : Controller
    {
        private readonly AppDbContext db;

        public DriverDashboardController(AppDbContext context)
        {
            db = context;
        }

        private bool IsDriver() => HttpContext.Session.GetString("Role") == "Driver";
        private int GetDriverID() => int.TryParse(HttpContext.Session.GetString("UserID"), out int id) ? id : 0;

        // ✅ DASHBOARD - Matches image_ca8d81.jpg layout
        public IActionResult Dashboard()
        {
            if (!IsDriver()) return RedirectToAction("Login", "Auth");

            var driverId = GetDriverID();
            var deliveries = db.Deliveries
                .Where(d => d.DriverID == driverId)
                .OrderByDescending(d => d.CreatedDate)
                .ToList();

            return View(deliveries);
        }

        // ✅ UPDATE STATUS - Triggered by dropdown in View
        [HttpPost]
        public IActionResult UpdateStatus(int deliveryId, string status)
        {
            if (!IsDriver()) return RedirectToAction("Login", "Auth");

            var delivery = db.Deliveries.Find(deliveryId);
            if (delivery != null)
            {
                delivery.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Dashboard");
        }

        public IActionResult Report()
        {
            if (!IsDriver()) return RedirectToAction("Login", "Auth");
            var deliveries = db.Deliveries.Where(d => d.DriverID == GetDriverID()).ToList();
            return View(deliveries);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}