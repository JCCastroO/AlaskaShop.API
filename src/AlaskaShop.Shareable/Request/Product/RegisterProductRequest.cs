using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Response.Product;
using MediatR;
using OperationResult;

namespace AlaskaShop.Shareable.Request.Product;

public record RegisterProductRequest(RegisterProductDto Data, Guid UserIdentifier) : IRequest<Result<RegisterProductResponse>>;
