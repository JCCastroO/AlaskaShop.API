using AlaskaShop.Domain.Services.Validation.Auth;
using AlaskaShop.Domain.Services.Validation.Product;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Product.Register;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using AutoMapper;
using MediatR;
using OperationResult;

namespace AlaskaShop.Domain.Handler.Product;

public class RegisterProductHandler : IRequestHandler<RegisterProductRequest, Result<RegisterProductResponse>>
{
    private readonly IRegisterProductRepository _repository;
    private readonly IMapper _mapper;

    public RegisterProductHandler(IRegisterProductRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<RegisterProductResponse>> Handle(RegisterProductRequest request, CancellationToken cancellationToken)
    {
        var valid = Validate(request.Data);
        if (!valid)
            return new ApplicationException("Request inválido!");

        var newProduct = _mapper.Map<ProductEntity>(request.Data);
        newProduct.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        newProduct.CreatedBy = request.UserIdentifier;

        try
        {
            await _repository.RegisterNewProduct(newProduct);
        }
        catch (Exception)
        {
            return new ApplicationException("Erro ao registrar novo produto!");
        }

        return new RegisterProductResponse("Produto registrado com sucesso!");
    }

    private static bool Validate(RegisterProductDto data)
    {
        var validator = new RegisterProductValidation();
        var result = validator.Validate(data);

        if (!result.IsValid)
            return false;

        return true;
    }
}
