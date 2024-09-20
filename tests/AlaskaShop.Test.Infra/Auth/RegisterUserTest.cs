using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Auth;
using AlaskaShop.Infra.Repositories.Auth.Register;
using Bogus;
using FluentAssertions;

namespace AlaskaShop.Test.Infra.Auth;

public class RegisterUserTest : TestApp
{
    private readonly RegisterUserRepository _repository;

    public RegisterUserTest()
        => _repository = new(_context);

    [Fact]
    public async Task VerifyExistingEmail_Success()
    {
        // Arrange
        var user = UserBuilder();

        // Act
        var result = await _repository.VerifyExistingEmail(user.Email);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task VerifyExistingEmail_Error()
    {
        // Arrange
        var user = UserBuilder();
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.VerifyExistingEmail(user.Email);

        // Assert
        result.Should().NotBeNull();
    }

    private static UserEntity UserBuilder()
        => new Faker<UserEntity>()
        .RuleFor(u => u.Name, f => f.Person.FirstName)
        .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Name))
        .RuleFor(u => u.Password, f => f.Internet.Password(6))
        .RuleFor(u => u.CreatedAt, f => DateOnly.FromDateTime(DateTime.UtcNow))
        .RuleFor(u => u.Active, f => true);
}
