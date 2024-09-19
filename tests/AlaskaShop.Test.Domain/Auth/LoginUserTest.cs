using AlaskaShop.Domain.Handler.Auth;
using AlaskaShop.Domain.Services.Crypto;
using AlaskaShop.Domain.Services.Token;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Auth;
using AlaskaShop.Infra.Repositories.Auth.Login;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Response.Auth;
using AlaskaShop.Shareable.Vos.Auth;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;

namespace AlaskaShop.Test.Domain.Auth;

public class LoginUserTest : TestApp
{
    private readonly RegisterUserRepository _register;
    private readonly LoginUserRepository _repository;
    private readonly LoginUserHandler _handler;
    private readonly PasswordEncrypter _encrypter;
    private readonly JwtTokenGenerator _token;

    public LoginUserTest()
    {
        _register = new(_context);
        _repository = new(_context);
        _encrypter = new("@Test");
        _token = new("Test#@#Test#@#123#@#456#@#789#@#Test", 600);
        _handler = new(_repository, _encrypter, _token);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var user = UserBuilder(true);
        var request = new LoginUserRequest(RequestBuilder(user.Password, user.Email));
        user.Password = _encrypter.Encrypt(user.Password);
        await _register.RegisterNewUser(user);
        var existingUser = await _repository.VerifyExistingUser(user.Email, user.Password);
        var fakeResponse = new LoginUserVo()
        {
            Name = existingUser!.Name,
            AccessToken = _token.Generate(existingUser!.UserIdentifier)
        };

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Exception.Should().BeNull();
        response.Value.Should().NotBeNull();
        response.Value!.Data.Name.Should().Be(fakeResponse.Name);
        response.Value!.Data.AccessToken.Should().Be(fakeResponse.AccessToken);
    }

    [Theory]
    [InlineData("joao@email.com", "")]
    [InlineData("joao", "123456")]
    [InlineData("", "123456")]
    public async Task Validation_Error(string email, string password)
    {
        // Arrange
        var request = new LoginUserRequest(RequestBuilder(password, email));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Request inválido!");
    }

    [Fact]
    public async Task NotExistingUser_Error()
    {
        // Arrange
        var user = UserBuilder(true);
        var request = new LoginUserRequest(RequestBuilder(user.Password, user.Email));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Usuário não encontrado!");
    }

    [Fact]
    public async Task NotActiveUser_Error()
    {
        // Arrange
        var user = UserBuilder(false);
        var request = new LoginUserRequest(RequestBuilder(user.Password, user.Email));
        user.Password = _encrypter.Encrypt(user.Password);
        await _register.RegisterNewUser(user);

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Usuário inativo!");
    }

    private static LoginUserDto RequestBuilder(string password, string? email)
            => new Faker<LoginUserDto>()
            .RuleFor(u => u.Email, (f, u) => email)
            .RuleFor(u => u.Password, f => password);

    private static UserEntity UserBuilder(bool active)
        => new Faker<UserEntity>()
        .RuleFor(u => u.Name, f => f.Person.FirstName)
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
        .RuleFor(u => u.Password, f => f.Internet.Password(6))
        .RuleFor(u => u.CreatedAt, f => DateOnly.FromDateTime(DateTime.UtcNow))
        .RuleFor(u => u.Active, f => active);
}
