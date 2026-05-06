using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebApp.Common.Validation;

public sealed class PasswordPolicyAttribute : ValidationAttribute
{
    private static readonly Regex PasswordRegex = new(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{8,}$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public override bool IsValid(object? value)
    {
        if (value is not string password)
        {
            return false;
        }

        // At least 8 chars, one uppercase, one lowercase, one digit, one special char.
        return PasswordRegex.IsMatch(password);
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be at least 8 characters and include uppercase, lowercase, number, and special character.";
    }

}

