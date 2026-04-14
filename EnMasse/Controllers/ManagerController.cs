using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using EnMasse.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace EnMasse.Controllers
{
    public class ManagerController : Controller
    {
        private readonly AppDbContext db;

        // Inject the database context
        public ManagerController(AppDbContext context)
        {
            db = context;
        }

        // Security Helper
        private bool IsManager()
        {
            return HttpContext.Session.GetString("Role") == "Manager";
        }

        public IActionResult Dashboard(string searchString)
        {
            if (!IsManager()) return RedirectToAction("Login", "Auth");

            // Pull deliveries from the DB and include the User data so we can see the Client name
            var dbDeliveries = db.Deliveries.Include(d => d.User).ToList();

            // Map the DB Deliveries into the 'Order' format your front-end already expects!
            var orders = dbDeliveries.Select(d => new Order
            {
                ReqId = d.DeliveryID,
                Client = d.User != null ? d.User.Username : "Unknown Client",
                Destination = d.DeliveryAddress,
                Status = d.Status
            }).ToList();

            // Apply Search Filter if used
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o => o.Client.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
                                           o.ReqId.ToString() == searchString).ToList();
            }

            var model = new DashboardViewModel
            {
                ManagerName = HttpContext.Session.GetString("Username") ?? "Manager",
                ActiveDrivers = 13, // Still hardcoded visually for now
                PendingRequests = orders.Count(o => o.Status == "Pending"),
                InTransit = orders.Count(o => o.Status == "In Transit"),
                RecentOrders = orders.OrderByDescending(o => o.ReqId).ToList()
            };

            return View(model);
        }

        public IActionResult OrderDetails(int id)
        {
            if (!IsManager()) return RedirectToAction("Login", "Auth");

            var delivery = db.Deliveries.Include(d => d.User).FirstOrDefault(d => d.DeliveryID == id);

            if (delivery == null) return NotFound();

            // Map it for the View
            var order = new Order
            {
                ReqId = delivery.DeliveryID,
                Client = delivery.User != null ? delivery.User.Username : "Unknown Client",
                Destination = delivery.DeliveryAddress,
                Status = delivery.Status
            };

            return View(order);
        }

        [HttpGet]
        public IActionResult CreateTrip()
        {
            if (!IsManager()) return RedirectToAction("Login", "Auth");
            return View();
        }

        [HttpPost]
        public IActionResult CreateTrip(string clientName, string destination)
        {
            if (!IsManager()) return RedirectToAction("Login", "Auth");

            // DB RELATIONAL LOGIC: A Delivery MUST belong to a User in the database.
            // We search for a user that matches the typed client name.
            var targetUser = db.Users.FirstOrDefault(u => u.Username.Contains(clientName));

            // If the manager types a name that isn't registered, we just assign it to the first available user 
            // so the database doesn't crash from a missing Foreign Key.
            if (targetUser == null)
            {
                targetUser = db.Users.FirstOrDefault();
            }

            // Prevent crash if the database is 100% empty of users
            if (targetUser == null)
            {
                ViewBag.Error = "Cannot create trip: There are no registered customers in the database yet.";
                return View();
            }

            // Create the new Delivery entity
            var newDelivery = new Delivery
            {
                PickupAddress = "En Massé Main Warehouse", // Default
                DeliveryAddress = destination,
                DescriptionOfGoods = "Manager Dispatched Goods",
                Weight = "TBD",
                DeliveryDate = DateTime.Now.AddDays(1),
                Status = "Pending",
                CreatedDate = DateTime.Now,
                UserID = targetUser.UserID
            };

            // Save to actual SQL database
            db.Deliveries.Add(newDelivery);
            db.SaveChanges();

            return RedirectToAction("Dashboard");
        }
    }
}