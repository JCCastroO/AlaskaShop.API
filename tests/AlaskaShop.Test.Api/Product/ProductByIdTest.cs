using FluentAssertions;
using System.Net;

namespace AlaskaShop.Test.Api.Product;

public class ProductByIdTest : IClassFixture<TestApp>
{
    private readonly HttpClient _httpClient;
    public ProductByIdTest(TestApp factory)
        => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Unauthorized_Error()
    {
        // Arrange

        // Act
        var response = await _httpClient.GetAsync($"/api/v1/product/{1}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
