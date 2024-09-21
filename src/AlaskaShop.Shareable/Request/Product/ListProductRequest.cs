using AlaskaShop.Shareable.Dtos;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Response.Product;
using MediatR;
using OperationResult;

namespace AlaskaShop.Shareable.Request.Product;

public record ListProductRequest(ListProductDto FilterParams, PaginationDto PageParams) : IRequest<Result<ListProductResponse>>;
