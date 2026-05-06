using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnMasse.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryID { get; set; }

        [Required]
        public string PickupAddress { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        [Required]
        public string DescriptionOfGoods { get; set; }

        [Required]
        public string Weight { get; set; }

        [Required]
        public DateTime DeliveryDate { get; set; }

        public string? SpecialInstructions { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // ✅ CUSTOMER
        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }

        // ✅ DRIVER (🔥 THIS FIXES YOUR ERROR)
        public int? DriverID { get; set; }

        [ForeignKey("DriverID")]
        public virtual Driver? Driver { get; set; }
    }
}