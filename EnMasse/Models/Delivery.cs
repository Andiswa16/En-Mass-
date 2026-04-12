using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnMasse.Models
{
    public class Delivery
    {
        [Key]
        public int DeliveryID { get; set; }

        [Required(ErrorMessage = "Pickup address is required.")]
        public string PickupAddress { get; set; }

        [Required(ErrorMessage = "Delivery address is required.")]
        public string DeliveryAddress { get; set; }

        [Required(ErrorMessage = "Description of goods is required.")]
        public string DescriptionOfGoods { get; set; }

        [Required(ErrorMessage = "Weight is required.")]
        public string Weight { get; set; }

        [Required(ErrorMessage = "Delivery date is required.")]
        public DateTime DeliveryDate { get; set; }

        public string? SpecialInstructions { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int UserID { get; set; }

        [ForeignKey("UserID")]
        public virtual User? User { get; set; }
    }
}