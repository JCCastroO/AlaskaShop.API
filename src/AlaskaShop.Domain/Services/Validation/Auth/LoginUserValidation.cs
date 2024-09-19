using AlaskaShop.Shareable.Dtos.Auth;
using FluentValidation;

namespace AlaskaShop.Domain.Services.Validation.Auth;

public class LoginUserValidation : AbstractValidator<LoginUserDto>
{
    public LoginUserValidation()
    {
        RuleFor(u => u.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(u => u.Password).NotEmpty().NotNull();
        RuleFor(u => u.Password.Length).GreaterThanOrEqualTo(6);
    }
}
