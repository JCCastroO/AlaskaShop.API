using AlaskaShop.Shareable.Dtos.Auth;
using Bogus;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Enums;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Logging;

namespace AlaskaShop.Test.Api.Product;

public class RegisterProductTest : IClassFixture<TestApp>
{
    private readonly HttpClient _httpClient;
    public RegisterProductTest(TestApp factory) => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Unauthorized_Error()
    {
        // Arrange
        var request = RequestBuilder(null);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/v1/product/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static RegisterProductDto RequestBuilder(string? name)
       => new Faker<RegisterProductDto>()
       .RuleFor(p => p.Name, f => name ?? f.Lorem.Word())
       .RuleFor(p => p.Price, f => f.Random.Double(1.00, 100.00))
       .RuleFor(p => p.Image, f => f.Random.String())
       .RuleFor(p => p.Type, f => ProductTypeEnum.Scarf);
}
