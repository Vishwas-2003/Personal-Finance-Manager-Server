using System.ComponentModel.DataAnnotations;
using WebApp.Common.Validation;

namespace WebApp.Common.Models.Auth;

public class RegisterRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [PasswordPolicy]
    public string Password { get; set; } = string.Empty;
}

