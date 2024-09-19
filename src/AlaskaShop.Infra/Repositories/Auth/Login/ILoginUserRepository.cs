using AlaskaShop.Infra.Entities;

namespace AlaskaShop.Infra.Repositories.Auth.Login;

public interface ILoginUserRepository
{
    Task<UserEntity?> VerifyExistingUser(string email, string password);
}
