using System.ComponentModel.DataAnnotations;

public class GuidNotEmptyAttribute : ValidationAttribute
{
    public GuidNotEmptyAttribute() : base("O formato do campo {0} é inválido.") { }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is Guid guid && guid != Guid.Empty)
            return ValidationResult.Success;

        return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
    }
}
