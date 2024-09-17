using AlaskaShop.Domain.Handler.Auth;
using AlaskaShop.Infra;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Auth;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Response.Auth;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using OperationResult;

namespace AlaskaShop.Test.Domain.Auth;

public class RegisterUserTest : TestApp
{
    private readonly RegisterUserRepository _repository;
    private readonly RegisterUserHandler _handler;

    public RegisterUserTest()
    {
        _repository = new(_context);
        _handler = new(_repository, _mapper);
    }

    [Fact]
    public async Task Success()
    {
        // Arrange
        var request = new RegisterUserRequest(RequestBuilder(6, null, null));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Exception.Should().BeNull();
        response.Value.Should().Be(new RegisterUserResponse("Cadastro realizado com sucesso!"));
    }

    [Theory]
    [InlineData(3, "joao", "joao@email.com")]
    [InlineData(6, "", "joao@email.com")]
    [InlineData(6, "joao", "")]
    [InlineData(6, "joao", "joao")]
    public async Task Validation_Error(int passwordLength, string name, string email)
    {
        // Arrange
        var request = new RegisterUserRequest(RequestBuilder(passwordLength, name, email));

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Request inválido!");
    }

    [Fact]
    public async Task ExistingUser_Error()
    {
        // Arrange
        var request = new RegisterUserRequest(RequestBuilder(6, null, null));
        var user = _mapper.Map<UserEntity>(request.Data);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var response = await _handler.Handle(request, new CancellationToken());

        // Assert
        response.Value.Should().BeNull();
        response.Exception.Should().NotBeNull();
        response.Exception?.GetType().Should().Be(typeof(ApplicationException));
        response.Exception?.Message.Should().Be("Usuário já cadastrado!");
    }

    private static RegisterUserDto RequestBuilder(int passwordLength, string? name, string? email)
        => new Faker<RegisterUserDto>()
        .RuleFor(u => u.Name, f => name is null ? f.Person.FirstName : name)
        .RuleFor(u => u.Email, (f, u) => email is null ? f.Internet.Email(u.Name) : email)
        .RuleFor(u => u.Password, f => f.Internet.Password(passwordLength));
}
