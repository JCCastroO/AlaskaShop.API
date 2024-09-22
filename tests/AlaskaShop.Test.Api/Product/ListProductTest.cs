using System.Net.Http.Json;
using System.Net;
using AlaskaShop.Shareable.Dtos.Product;
using AlaskaShop.Shareable.Dtos;
using Bogus;
using NSubstitute;
using System.Web;
using FluentAssertions;

namespace AlaskaShop.Test.Api.Product;

public class ListProductTest : IClassFixture<TestApp>
{
    private readonly HttpClient _httpClient;
    public ListProductTest(TestApp factory)
        => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Unauthorized_Error()
    {
        // Arrange
        var request = RequestBuilder();

        // Act
        var response = await _httpClient.GetAsync($"/api/v1/product/list?{request}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    private static string RequestBuilder()
    {
        var filter = RequestListBuilder();
        var filterProps = filter.GetType().GetProperties()
            .Where(p => p.GetValue(filter, null) != null && !string.IsNullOrWhiteSpace(p.GetValue(filter, null)!.ToString()))
            .Select(p => p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(filter, null)!.ToString()));
        var filterQuery = string.Join("&", filterProps.ToArray());
        var pagination = RequestPaginationBuilder();
        var paginationProps = pagination.GetType().GetProperties()
            .Where(p => p.GetValue(pagination, null) != null)
            .Select(p => p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(pagination, null)!.ToString())); 
        var paginationQuery = string.Join("&", paginationProps.ToArray());
        return filterQuery + "&" + paginationQuery;
    }

    private static ListProductDto RequestListBuilder()
        => new Faker<ListProductDto>()
        .RuleFor(p => p.MaxPrice, f => f.Random.Double(50.00, 100.00));

    private static PaginationDto RequestPaginationBuilder()
        => new Faker<PaginationDto>()
        .RuleFor(p => p.Page, f => 1)
        .RuleFor(p => p.PageSize, f => 25);
}
