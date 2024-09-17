using AlaskaShop.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Infra.Repositories.Auth;

public class RegisterUserRepository : IRegisterUserRepository
{
    private readonly Context _context;

    public RegisterUserRepository(Context context)
        => _context = context;

    public async Task RegisterNewUser(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<UserEntity?> VerifyExistingEmail(string email)
        => await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}
