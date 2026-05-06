using System.Collections.Generic;
using EnMasse.Models;

namespace EnMasse.Models
{
    public class AdminDashboardViewModel
    {
        public string AdminName { get; set; }

        public int TotalClients { get; set; }
        public int TotalDeliveries { get; set; }
        public int PendingDeliveries { get; set; }
        public int InTransitDeliveries { get; set; }
        public int CompletedDeliveries { get; set; }

        public int ActiveDrivers { get; set; }

        public List<string> FleetVehicles { get; set; }
        public List<string> ClientNames { get; set; }

        public List<Delivery> AllDeliveries { get; set; }
        public List<User> AllClients { get; set; }
    }
}