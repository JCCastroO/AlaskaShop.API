using AlaskaShop.Domain.Services.Validation.Product;
using AlaskaShop.Infra.Repositories.Product.ById;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using AlaskaShop.Shareable.Vos.Product;
using AutoMapper;
using MediatR;
using OperationResult;

namespace AlaskaShop.Domain.Handler.Product;

public class ProductByIdHandler : IRequestHandler<ProductByIdRequest, Result<ProductByIdResponse>>
{
    private readonly IProductByIdRepository _repository;
    private readonly IMapper _mapper;

    public ProductByIdHandler(IProductByIdRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<Result<ProductByIdResponse>> Handle(ProductByIdRequest request, CancellationToken cancellationToken)
    {
        var valid = request.Id > 0;
        if (!valid)
            return new ApplicationException("Request inválido");

        var product = await _repository.GetProduct(request.Id);
        if (product is null)
            return new ApplicationException("Produto não encontrado!");

        var item = _mapper.Map<ProductByIdVo>(product);
        return new ProductByIdResponse(item);
    }
}
