using System.ComponentModel.DataAnnotations;
namespace EnMasse.Models;

public class Delivery
{
    [Key]
    public int DeliveryID { get; set; }

    public string PickupAddress { get; set; }
    public string DeliveryAddress { get; set; }
    public string DescriptionOfGoods { get; set; }
    public string Weight { get; set; }
    public DateTime DeliveryDate { get; set; }
    public string SpecialInstructions { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public int UserID { get; set; }
    public virtual User User { get; set; }
}