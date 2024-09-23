using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Product.ById;
using AlaskaShop.Shareable.Enums;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Vos.Product;
using Bogus;
using FluentAssertions;
using System.Reflection.Metadata;

namespace AlaskaShop.Test.Infra.Product;

public class ProductByIdTest : TestApp
{
    private readonly ProductByIdRepository _repository;
    public ProductByIdTest()
        => _repository = new(_context);


    [Fact]
    public async Task Success_NotNull()
    {
        // Arrange
        var product = ProductBuilder(1);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var response = await _repository.GetProduct(product.Id);

        // Assert
        response.Should().NotBeNull();
        response!.Id.Should().Be(product.Id);
    }

    [Fact]
    public async Task Success_Null()
    {
        // Arrange
        var product = ProductBuilder(1);
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        // Act
        var response = await _repository.GetProduct(2);

        // Assert
        response.Should().BeNull();
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
