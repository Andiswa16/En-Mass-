namespace EnMasse.Models
{
    public class Driver
    {
        public int Id { get; set; }

        public string DUsername { get; set; }

        public string DPassword { get; set; }

        // ✅ LINK TO DELIVERIES
        public List<Delivery>? Deliveries { get; set; }
    }
}