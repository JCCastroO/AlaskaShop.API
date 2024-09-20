using AlaskaShop.Shareable.Dtos.Product;
using FluentValidation;

namespace AlaskaShop.Domain.Services.Validation.Product;

public class RegisterProductValidation : AbstractValidator<RegisterProductDto>
{
    public RegisterProductValidation()
    {
        RuleFor(p => p.Name).NotEmpty().NotNull();
        RuleFor(p => p.Price).NotEmpty().NotNull().GreaterThan(0.9);
        RuleFor(p => p.Image).NotEmpty().NotNull();
        RuleFor(p => p.Type).NotEmpty().NotNull().IsInEnum();
    }
}
