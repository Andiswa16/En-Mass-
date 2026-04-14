namespace EnMasse.Models
{
    // The main model passed to the View
    public class DashboardViewModel
    {
        public string ManagerName { get; set; } = "Jane Doe";
        public int ActiveDrivers { get; set; }
        public int PendingRequests { get; set; }
        public int InTransit { get; set; }
        public List<Order> RecentOrders { get; set; } = new List<Order>();
    }
}