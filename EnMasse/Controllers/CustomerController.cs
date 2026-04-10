using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnMasse.Models;
using System.Linq;

namespace EnMasse.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext db;

        public CustomerController(AppDbContext context)
        {
            db = context;
        }

        private bool IsLoggedIn()
        {
            return HttpContext.Session.GetString("UserID") != null;
        }

        private int GetUserID()
        {
            var userId = HttpContext.Session.GetString("UserID");
            return userId != null ? int.Parse(userId) : 0;
        }

        // DASHBOARD
        public IActionResult Index()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries
                .Where(d => d.UserID == GetUserID())
                .OrderByDescending(d => d.CreatedDate)
                .ToList();

            return View(deliveries);
        }

        // REQUEST DELIVERY - GET
        public IActionResult RequestDelivery()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");
            return View();
        }

        // REQUEST DELIVERY - POST
        [HttpPost]
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

        // DELIVERY STATUS
        public IActionResult DeliveryStatus()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login", "Auth");

            var deliveries = db.Deliveries
                .Where(d => d.UserID == GetUserID())
                .ToList();

            return View(deliveries);
        }
    }
}