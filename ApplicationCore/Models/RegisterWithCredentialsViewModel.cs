using System.ComponentModel.DataAnnotations;

namespace ApplicationCore.Models;

public class RegisterWithCredentialsViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required]
    [Display(Name = "Password")]
    public string Password { get; set; }
}