using System.ComponentModel.DataAnnotations;
namespace EnMasse.Models;
public class User
{
    [Key]
    public int UserID { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string Role { get; set; } // Customer, Driver, Admin, Manager

    // FK
    public int RegistrationID { get; set; }
    public virtual Registration Registration { get; set; }
}