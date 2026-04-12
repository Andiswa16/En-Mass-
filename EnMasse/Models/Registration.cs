using System.ComponentModel.DataAnnotations;

namespace EnMasse.Models
{
    public class Registration
    {
        [Key]
        public int RegistrationID { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "VAT number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "VAT number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "VAT number must be 10 digits.")]
        public string VATNumber { get; set; }

        [Required(ErrorMessage = "Company address is required.")]
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Contact name is required.")]
        public string ContactName { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone number must be exactly 10 digits.")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits.")]
        public string ContactPhone { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string ContactEmail { get; set; }
    }
}