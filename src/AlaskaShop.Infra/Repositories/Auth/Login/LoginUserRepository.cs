using AlaskaShop.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Infra.Repositories.Auth.Login;

public class LoginUserRepository : ILoginUserRepository
{
    private readonly Context _context;

    public LoginUserRepository(Context context)
        => _context = context;

    public async Task<UserEntity?> VerifyExistingUser(string email, string password)
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
}
