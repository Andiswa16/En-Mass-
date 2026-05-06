using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnMasse.Models;
using EnMasse.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace EnMasse.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext db;

        public AdminController(AppDbContext context)
        {
            db = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        // =========================
        // ADMIN DASHBOARD
        // =========================
        public IActionResult Dashboard(string searchString)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries.Include(d => d.User).ToList();
            var users = db.Users.ToList();

            // 🔍 SEARCH
            if (!string.IsNullOrEmpty(searchString))
            {
                deliveries = deliveries
                    .Where(d => d.User != null &&
                           d.User.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // 🚛 MOCK FLEET (for your UI cards)
            var fleet = new List<string>
            {
                "Mercedes-Benz Actros 1845",
                "Mercedes-Benz Arocs",
                "Mercedes-Benz Atego",
                "Mercedes-Benz Axor",
                "Mercedes-Benz eActros"
            };

            // 🏢 CLIENT LIST (Checkers, etc.)
            var clientNames = users
                .Where(u => u.Role == "Customer")
                .Select(u => u.Username)
                .ToList();

            var model = new AdminDashboardViewModel
            {
                AdminName = HttpContext.Session.GetString("Username") ?? "Admin",

                TotalClients = users.Count(u => u.Role == "Customer"),
                TotalDeliveries = deliveries.Count,
                PendingDeliveries = deliveries.Count(d => d.Status == "Pending"),
                InTransitDeliveries = deliveries.Count(d => d.Status == "In Transit"),
                CompletedDeliveries = deliveries.Count(d => d.Status == "Delivered"),

                // 🔥 FIXED (no MockDriverData)
                ActiveDrivers = 1, // since you only have 1 driver mock

                FleetVehicles = fleet,
                ClientNames = clientNames,

                AllDeliveries = deliveries.OrderByDescending(d => d.DeliveryID).ToList(),
                AllClients = users.Where(u => u.Role == "Customer").ToList()
            };

            return View(model);
        }

        // =========================
        // MANAGE CLIENTS
        // =========================
        public IActionResult Clients(string searchString)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Auth");

            var clients = db.Users
                .Where(u => u.Role == "Customer")
                .Include(u => u.Registration)
                .ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients
                    .Where(u => u.Username.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return View(clients);
        }

        // =========================
        // MANAGE DRIVERS (SIMPLE)
        // =========================
        public IActionResult Drivers()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Auth");

            // 🔥 SIMPLE MOCK DRIVER LIST (since we removed MockDriverData)
            var drivers = new List<string>
            {
                "driver1"
            };

            return View(drivers);
        }

        // =========================
        // MANAGE DELIVERIES
        // =========================
        public IActionResult Deliveries(string searchString, string statusFilter)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries.Include(d => d.User).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                deliveries = deliveries.Where(d =>
                    d.User != null && d.User.Username.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(statusFilter))
            {
                deliveries = deliveries.Where(d => d.Status == statusFilter);
            }

            ViewBag.StatusFilter = statusFilter;
            ViewBag.SearchString = searchString;

            return View(deliveries.OrderByDescending(d => d.DeliveryID).ToList());
        }

        // =========================
        // UPDATE DELIVERY STATUS
        // =========================
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Auth");

            var delivery = db.Deliveries.FirstOrDefault(d => d.DeliveryID == id);
            if (delivery == null) return NotFound();

            delivery.Status = status;
            db.SaveChanges();

            TempData["Success"] = $"Delivery #{id} updated to '{status}'.";
            return RedirectToAction("Deliveries");
        }

        // =========================
        // DELETE CLIENT
        // =========================
        [HttpPost]
        public IActionResult DeleteClient(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Auth");

            var user = db.Users.FirstOrDefault(u => u.UserID == id);
            if (user == null) return NotFound();

            db.Users.Remove(user);
            db.SaveChanges();

            TempData["Success"] = "Client removed successfully.";
            return RedirectToAction("Clients");
        }
    }
}