namespace EnMasse.Models
{
    // Represents the Data Layer entity you'll eventually pull from a DB
    public class Order
    {
        public int ReqId { get; set; }
        public string Client { get; set; }
        public string Destination { get; set; }
        public string Status { get; set; }
    }
}