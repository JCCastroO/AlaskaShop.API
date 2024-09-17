using AlaskaShop.Shareable.Dtos.Auth;
using FluentValidation;

namespace AlaskaShop.Domain.Services.Validation.Auth;

public class RegisterUserValidation : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidation()
    {
        RuleFor(u => u.Name).NotEmpty().NotNull();
        RuleFor(u => u.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(u => u.Password).NotEmpty().NotNull();
        RuleFor(u => u.Password.Length).GreaterThanOrEqualTo(6);
    }
}
