using AlaskaShop.Domain.Handler.Product;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Product.List;
using AlaskaShop.Shareable.Dtos;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Enums;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using AlaskaShop.Shareable.Vos;
using AlaskaShop.Shareable.Vos.Product;
using Bogus;
using FluentAssertions;

namespace AlaskaShop.Test.Domain.Product;

public class ListProductTest : TestApp
{
    private readonly ListProductHandler _handler;
    private readonly ListProductRepository _repository;

    public ListProductTest()
    {
        _repository = new(_context);
        _handler = new(_repository);
    }

    [Fact]
    public async Task Success_NotVoid()
    {
        // Arrange
        var product = ProductBuilder();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        var request = new ListProductRequest(RequestListBuilder(50.00, 150.00), RequestPaginationBuilder(1, 25));
        var fakeListResponse = new List<ListProductVo>()
        {
            new()
            {
            Name = product.Name,
            Image = product.Image,
            Price = product.Price,
            }
        }.ToArray();
        var fakePaginationResponse = new PaginationVo()
        {
            MaxPage = 1,
            TotalItems = 1
        };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Exception.Should().BeNull();
        response.Value.Should().NotBeNull();
        response.Value!.List[0].Image.Should().Be(fakeListResponse[0].Image);
        response.Value!.List[0].Name.Should().Be(fakeListResponse[0].Name);
        response.Value!.List[0].Price.Should().Be(fakeListResponse[0].Price);
        response.Value!.PageInfo.MaxPage.Should().Be(fakePaginationResponse.MaxPage);
        response.Value!.PageInfo.TotalItems.Should().Be(fakePaginationResponse.TotalItems);
    }

    [Fact]
    public async Task Success_Void()
    {
        // Arrange
        var request = new ListProductRequest(RequestListBuilder(110.00, 150.00), RequestPaginationBuilder(1, 25));
        var fakePaginationResponse = new PaginationVo()
        {
            MaxPage = 1,
            TotalItems = 0
        };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Exception.Should().BeNull();
        response.Value.Should().NotBeNull();
        response.Value!.List.Length.Should().Be(0);
        response.Value!.PageInfo.MaxPage.Should().Be(fakePaginationResponse.MaxPage);
        response.Value!.PageInfo.TotalItems.Should().Be(fakePaginationResponse.TotalItems);
    }

    [Theory]
    [InlineData(10.00, 20.00, 1, 25)]
    [InlineData(50.00, 150.00, 0, 25)]
    [InlineData(50.00, 150.00, 1, 0)]
    public async Task Validation_Error(double minMaxPrice, double maxMaxPrice, int page, int pageSize)
    {
        // Arrange
        var request = new ListProductRequest(RequestListBuilder(minMaxPrice, maxMaxPrice), RequestPaginationBuilder(page, pageSize));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Request inválido!");
    }

    private static ProductEntity ProductBuilder()
        => new Faker<ProductEntity>()
        .RuleFor(p => p.Name, f => f.Lorem.Word())
        .RuleFor(p => p.Price, f => f.Random.Double(1.00, 100.00))
        .RuleFor(p => p.Image, f => f.Random.String())
        .RuleFor(p => p.Type, f => ProductTypeEnum.Scarf)
        .RuleFor(p => p.CreatedBy, f => Guid.NewGuid())
        .RuleFor(p => p.CreatedAt, f => DateOnly.FromDateTime(DateTime.UtcNow));

    private static ListProductDto RequestListBuilder(double minMaxPrice, double maxMaxPrice)
        => new Faker<ListProductDto>()
        .RuleFor(p => p.MaxPrice, f => f.Random.Double(minMaxPrice, maxMaxPrice));

    private static PaginationDto RequestPaginationBuilder(int page, int pageSize)
        => new Faker<PaginationDto>()
        .RuleFor(p => p.Page, f => page)
        .RuleFor(p => p.PageSize, f => pageSize);


}
