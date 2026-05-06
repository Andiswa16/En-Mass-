using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnMasse.Models;
using EnMasse.Data;
using System.Linq;
using System;

namespace EnMasse.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext db;
        private static readonly Random _random = new Random();

        public CustomerController(AppDbContext context)
        {
            db = context;
        }

        private bool IsLoggedIn() =>
            HttpContext.Session.GetString("UserID") != null;

        private int GetUserID() =>
            int.TryParse(HttpContext.Session.GetString("UserID"), out int id) ? id : 0;

        // ✅ DASHBOARD
        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries
                .Where(d => d.UserID == GetUserID())
                .OrderByDescending(d => d.CreatedDate)
                .ToList();

            return View(deliveries);
        }

        // ✅ REQUEST DELIVERY GET
        public IActionResult RequestDelivery()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // ✅ REQUEST DELIVERY POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RequestDelivery(Delivery delivery)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            if (ModelState.IsValid)
            {
                delivery.UserID = GetUserID();
                delivery.Status = "Pending";
                delivery.CreatedDate = DateTime.Now;

                db.Deliveries.Add(delivery);
                db.SaveChanges();

                TempData["Success"] = "Delivery requested successfully!";
                return RedirectToAction("Index");
            }

            return View(delivery);
        }

        // ✅ DELIVERY STATUS
        public IActionResult DeliveryStatus()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries
                .Where(d => d.UserID == GetUserID())
                .OrderByDescending(d => d.CreatedDate)
                .ToList();

            return View(deliveries);
        }

        // ✅ REPORT
        public IActionResult Report()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries
                .Where(d => d.UserID == GetUserID())
                .OrderByDescending(d => d.CreatedDate)
                .ToList();

            return View(deliveries);
        }

        // ✅ LOG TICKET GET
        public IActionResult LogTicket()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // ✅ LOG TICKET POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogTicket(string Category, string RelatedDeliveryId,
            string Subject, string Priority, string Description)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            if (string.IsNullOrEmpty(Subject) || string.IsNullOrEmpty(Description))
            {
                ModelState.AddModelError("", "Subject and Description are required");
                return View();
            }

            string ticketNumber = $"#TKT-{DateTime.Now:yyyyMMdd}-{_random.Next(1000, 9999)}";

            // TODO: Save to DB later
            TempData["TicketSuccess"] = true;
            TempData["TicketNumber"] = ticketNumber;

            return RedirectToAction("LogTicket");
        }

        // ✅ HELP
        public IActionResult Help()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View("HelpSettings");
        }

        // ✅ SETTINGS
        public IActionResult Settings()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            ViewBag.Page = "Settings";
            return View("HelpSettings");
        }
    }
}