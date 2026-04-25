using System.ComponentModel.DataAnnotations;

namespace EnMasse.Models
{
    public class LoginViewModel
    {
        [Required]
        public string DUsername { get; set; }

        [Required]
        public string DPassword { get; set; }
    }
}