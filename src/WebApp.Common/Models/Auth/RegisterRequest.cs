using System.ComponentModel.DataAnnotations;
using WebApp.Common.Validation;

namespace WebApp.Common.Models.Auth;

public class RegisterRequest
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;
    [Required]
    [Phone]
    [MaxLength(20)]
    public string MobileNumber { get; set; } = string.Empty;
    [Required]
    [Range(1, 120)]
    public int Age { get; set; }
    [Required]
    [MaxLength(500)]
    public string Address { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [PasswordPolicy]
    public string Password { get; set; } = string.Empty;
}

