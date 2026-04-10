using System.ComponentModel.DataAnnotations;

namespace EnMasse.Models;

public class Registration
{
    [Key]
    public int RegistrationID { get; set; }

    [Required]
    public string CompanyName { get; set; }

    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "VAT number must be exactly 10 digits.")]
    public string VATNumber { get; set; }

    public string CompanyAddress { get; set; }

    [Required(ErrorMessage = "Please enter CEO/Director or relevant person.")]
    public string ContactName { get; set; }

    public string ContactEmail { get; set; }

    [Required]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be exactly 10 digits.")]
    public string ContactPhone { get; set; }

    public DateTime RegistrationDate { get; set; } = DateTime.Now;

    public virtual ICollection<User> Users { get; set; }
}