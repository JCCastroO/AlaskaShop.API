using AlaskaShop.Shareable.Dtos.Auth;
using Bogus;
using System.Net.Http.Json;
using System.Net;
using FluentAssertions;
using AlaskaShop.Infra;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Domain.Services.Crypto;
using NSubstitute.ExceptionExtensions;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;

namespace AlaskaShop.Test.Api.Auth;

public class LoginUserTest : IClassFixture<TestApp>
{
    private readonly HttpClient _httpClient;
    private readonly PasswordEncrypter _encrypter = new("@Test");

    public LoginUserTest(TestApp factory)
        => _httpClient = factory.CreateClient();

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = RequestBuilder();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/login", request);

        // Assert
        response.Should().NotBeNull();
    }

    [Fact]
    public async Task Error()
    {
        // Arrange
        var request = RequestBuilder();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/login", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    private static LoginUserDto RequestBuilder()
        => new Faker<LoginUserDto>()
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password(6));
}
