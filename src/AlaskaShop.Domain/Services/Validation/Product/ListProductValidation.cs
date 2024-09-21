using AlaskaShop.Shareable.Dtos.Product;
using FluentValidation;

namespace AlaskaShop.Domain.Services.Validation.Product;

public class ListProductValidation: AbstractValidator<ListProductDto>
{
    public ListProductValidation()
    {
        RuleFor(p => p.Type).IsInEnum();
        RuleFor(p => p.MaxPrice).GreaterThanOrEqualTo(50.00);
    }
}
