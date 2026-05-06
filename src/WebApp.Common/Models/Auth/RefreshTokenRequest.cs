using System.ComponentModel.DataAnnotations;

namespace WebApp.Common.Models.Auth;

public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}

