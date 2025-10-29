using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

public class MatchConfigUsernameAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var config = validationContext.GetService(typeof(IConfiguration)) as IConfiguration ??
            throw new InvalidOperationException("IConfiguration service is not available in the validation context.");

        var allowedUsername = config["JWT:AllowedAuthUsername"];

        if (value == null || !string.Equals(value.ToString(), allowedUsername, StringComparison.OrdinalIgnoreCase))
        {
            return new ValidationResult($"Username is invalid: {value}");
        }

        return ValidationResult.Success;
    }
}