using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Auth;
using AlaskaShop.Infra.Repositories.Auth.Login;
using Bogus;
using FluentAssertions;

namespace AlaskaShop.Test.Infra.Auth;

public class LoginUserTest : TestApp
{
    private readonly LoginUserRepository _repository;

    public LoginUserTest()
        => _repository = new(_context);

    [Fact]
    public async Task VerifyExistingUser_Success()
    {
        // Arrange
        var user = UserBuilder();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.VerifyExistingUser(user.Email, user.Password);

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public async Task VerifyExistingUser_Error()
    {
        // Arrange
        var user = UserBuilder();

        // Act
        var result = await _repository.VerifyExistingUser(user.Email, user.Password);

        // Assert
        result.Should().BeNull();
    }

    private static UserEntity UserBuilder()
        => new Faker<UserEntity>()
        .RuleFor(u => u.Name, f => f.Person.FirstName)
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
        .RuleFor(u => u.Password, f => f.Internet.Password(6))
        .RuleFor(u => u.CreatedAt, f => DateOnly.FromDateTime(DateTime.UtcNow))
        .RuleFor(u => u.Active, f => true);
}
