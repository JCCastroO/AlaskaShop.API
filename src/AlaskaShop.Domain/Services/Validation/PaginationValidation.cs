using AlaskaShop.Shareable.Dtos;
using FluentValidation;

namespace AlaskaShop.Domain.Services.Validation;

public class PaginationValidation : AbstractValidator<PaginationDto>
{
    public PaginationValidation()
    {
        RuleFor(p => p.Page).NotNull().NotEmpty().GreaterThanOrEqualTo(1);
        RuleFor(p => p.PageSize).NotNull().NotEmpty().GreaterThanOrEqualTo(25);
    }
}
