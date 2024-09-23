using AlaskaShop.Domain.Handler.Product;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Product.ById;
using AlaskaShop.Shareable.Enums;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using AlaskaShop.Shareable.Vos.Product;
using AutoMapper;
using Bogus;
using FluentAssertions;

namespace AlaskaShop.Test.Domain.Product;

public class ProductByIdTest : TestApp
{
    private readonly ProductByIdHandler _handler;
    private readonly ProductByIdRepository _repository;

    public ProductByIdTest()
    {
        _repository = new(_context);
        _handler = new(_repository, _mapper);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var product = ProductBuilder(1);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        var request = new ProductByIdRequest(product.Id);
        var fakeResponse = _mapper.Map<ProductByIdVo>(product);

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Exception.Should().BeNull();
        response.Value!.Item.Id.Should().Be(fakeResponse.Id);
    }

    [Fact]
    public async Task Error()
    {
        // Arrange
        var request = new ProductByIdRequest(1);

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Produto não encontrado!");
    }

    [Fact]
    public async Task Validation_Error()
    {
        // Arrange
        var request = new ProductByIdRequest(0);

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Request inválido!");
    }

    private static ProductEntity ProductBuilder(long id)
        => new Faker<ProductEntity>()
        .RuleFor(p => p.Id, f => id)
        .RuleFor(p => p.Name, f => f.Lorem.Word())
        .RuleFor(p => p.Price, f => f.Random.Double(1.00, 100.00))
        .RuleFor(p => p.Image, f => f.Random.String())
        .RuleFor(p => p.Type, f => ProductTypeEnum.Scarf)
        .RuleFor(p => p.CreatedBy, f => Guid.NewGuid())
        .RuleFor(p => p.CreatedAt, f => DateOnly.FromDateTime(DateTime.UtcNow));
}
