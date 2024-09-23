using AlaskaShop.Shareable.Response.Product;
using MediatR;
using OperationResult;

namespace AlaskaShop.Shareable.Request.Product;

public record ProductByIdRequest(long Id) : IRequest<Result<ProductByIdResponse>>;
