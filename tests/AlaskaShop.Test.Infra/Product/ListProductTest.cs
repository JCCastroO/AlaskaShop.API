using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Product.List;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Enums;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using Bogus;
using FluentAssertions;
using System.Reflection.Metadata;

namespace AlaskaShop.Test.Infra.Product;

public class ListProductTest : TestApp
{
    private readonly ListProductRepository _repository;

    public ListProductTest()
        => _repository = new(_context);

    [Fact]
    public async Task GetProducts_NotVoid()
    {
        // Arrange
        var product = ProductBuilder();
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var response = await _repository.GetProducts();

        // Assert
        response.Should().NotBeNull();
        response.Length.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task GetProducts_Void()
    {
        // Arrange

        // Act
        var response = await _repository.GetProducts();

        // Assert
        response.Should().NotBeNull();
        response.Length.Should().Be(0);
    }

    private static ProductEntity ProductBuilder()
        => new Faker<ProductEntity>()
        .RuleFor(p => p.Name, f => f.Lorem.Word())
        .RuleFor(p => p.Price, f => f.Random.Double(1.00, 100.00))
        .RuleFor(p => p.Image, f => f.Random.String())
        .RuleFor(p => p.Type, f => ProductTypeEnum.Scarf)
        .RuleFor(p => p.CreatedBy, f => Guid.NewGuid())
        .RuleFor(p => p.CreatedAt, f => DateOnly.FromDateTime(DateTime.UtcNow));
}
