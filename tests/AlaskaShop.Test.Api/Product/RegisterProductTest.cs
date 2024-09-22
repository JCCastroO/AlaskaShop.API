using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Enums;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace AlaskaShop.Test.Api.Product;

public class RegisterProductTest : IClassFixture<TestApp>
{
    private readonly HttpClient _httpClient;
    public RegisterProductTest(TestApp factory)
        => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Unauthorized_Error()
    {
        // Arrange
        var request = RequestBuilder();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/product/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static RegisterProductDto RequestBuilder()
       => new Faker<RegisterProductDto>()
       .RuleFor(p => p.Name, f => f.Lorem.Word())
       .RuleFor(p => p.Price, f => f.Random.Double(1.00, 100.00))
       .RuleFor(p => p.Image, f => f.Random.String())
       .RuleFor(p => p.Type, f => ProductTypeEnum.Scarf);
}
