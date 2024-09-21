using AlaskaShop.Domain.Services.Validation;
using AlaskaShop.Domain.Services.Validation.Product;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Product.List;
using AlaskaShop.Shareable.Dtos;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using AlaskaShop.Shareable.Vos;
using AlaskaShop.Shareable.Vos.Product;
using MediatR;
using OperationResult;

namespace AlaskaShop.Domain.Handler.Product;

public class ListProductHandler : IRequestHandler<ListProductRequest, Result<ListProductResponse>>
{
    private readonly IListProductRepository _repository;

    public ListProductHandler(IListProductRepository repository)
        => _repository = repository;

    public async Task<Result<ListProductResponse>> Handle(ListProductRequest request, CancellationToken cancellationToken)
    {
        var valid = Validate(request.FilterParams, request.PageParams);
        if (!valid)
            return new ApplicationException("Request inválido!");

        var products = await _repository.GetProducts();
        if (products.Length == 0)
            return new ListProductResponse([], new PaginationVo() { MaxPage = 1, TotalItems = 0 });

        var filtredList = FilterList(products, request.FilterParams);
        if (filtredList.Length == 0)
            return new ListProductResponse([], new PaginationVo() { MaxPage = 1, TotalItems = 0 });

        var list = ListBuilder(filtredList);
        var pageInfo = PageInfoBuilder(list, request.PageParams);

        return new ListProductResponse(list, pageInfo);
    }

    private static PaginationVo PageInfoBuilder(ListProductVo[] list, PaginationDto page)
        => new PaginationVo()
        {
            MaxPage = list.Length / page.PageSize < 1 ? (list.Length / page.PageSize) + 1 : list.Length / page.PageSize,
            TotalItems = list.Length,
        };

    private static ListProductVo[] ListBuilder(ProductEntity[] list)
        => (from Product in list
            select new ListProductVo()
            {
                Name = Product.Name,
                Price = Product.Price,
                Image = Product.Image
            }).ToArray();

    private static ProductEntity[] FilterList(ProductEntity[] list, ListProductDto filter)
        => list.Where(p
            => (string.IsNullOrEmpty(filter.Name) || p.Name.Contains(filter.Name))
        && (!filter.Type.HasValue || p.Type == filter.Type.Value)
        && (!filter.MinPrice.HasValue || p.Price >= filter.MinPrice.Value)
        && (!filter.MaxPrice.HasValue || p.Price <= filter.MaxPrice.Value)
        ).ToArray();

    private static bool Validate(ListProductDto filter, PaginationDto page)
    {
        var filterValidator = new ListProductValidation();
        var pageValidator = new PaginationValidation();
        var filterResult = filterValidator.Validate(filter);
        var pageResult = pageValidator.Validate(page);

        if (!filterResult.IsValid || !pageResult.IsValid)
            return false;

        return true;
    }
}
