using AlaskaShop.Domain.Handler.Product;
using AlaskaShop.Infra.Repositories.Product.Register;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Enums;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Request.Product;
using AlaskaShop.Shareable.Response.Product;
using Bogus;
using FluentAssertions;

namespace AlaskaShop.Test.Domain.Product;

public class RegisterProductTest : TestApp
{
    private readonly RegisterProductHandler _handler;
    private readonly RegisterProductRepository _repository;

    public RegisterProductTest()
    {
        _repository = new(_context);
        _handler = new(_repository, _mapper);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = new RegisterProductRequest(RequestBuilder(null, null, null, null), Guid.NewGuid());

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Exception.Should().BeNull();
        response.Value.Should().Be(new RegisterProductResponse("Produto registrado com sucesso!"));
    }

    [Theory]
    [InlineData("", 3.00, "string", ProductTypeEnum.Cap)]
    [InlineData("touca", 0, "string", ProductTypeEnum.Cap)]
    [InlineData("touca", 3.00, "", ProductTypeEnum.Cap)]
    public async Task Validation_Error(string name, double price, string image, ProductTypeEnum type)
    {
        // Arrange
        var request = new RegisterProductRequest(RequestBuilder(name, price, image, type), Guid.NewGuid());

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Request inválido!");
    }

    private static RegisterProductDto RequestBuilder(string? name, double? price, string? image, ProductTypeEnum? type)
        => new Faker<RegisterProductDto>()
        .RuleFor(p => p.Name, f => name ?? f.Lorem.Word())
        .RuleFor(p => p.Price, f => price ?? f.Random.Double(1.00, 100.00))
        .RuleFor(p => p.Image, f => image ?? f.Random.String())
        .RuleFor(p => p.Type, f => type ?? ProductTypeEnum.Scarf);
}
