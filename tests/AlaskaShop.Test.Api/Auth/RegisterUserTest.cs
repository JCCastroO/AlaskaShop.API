using AlaskaShop.Shareable.Dtos.Auth;
using Bogus;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace AlaskaShop.Test.Api.Auth;

public class RegisterUserTest : IClassFixture<TestApp>
{
    private readonly HttpClient _httpClient;
    public RegisterUserTest(TestApp factory)
        => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestBuilder(6);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/register", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Error()
    {
        // Arrange
        var request = RequestBuilder(3);

        // Act
        var response = await _httpClient.PostAsJsonAsync("/register", request);

        // Assert
        response.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }

    private static RegisterUserDto RequestBuilder(int passwordLength)
        => new Faker<RegisterUserDto>()
        .RuleFor(u => u.Name, f => f.Person.FirstName)
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
        .RuleFor(u => u.Password, f => f.Internet.Password(passwordLength));
}
