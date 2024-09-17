using AlaskaShop.Infra.Entities;

namespace AlaskaShop.Infra.Repositories.Auth;

public interface IRegisterUserRepository
{
    Task<UserEntity?> VerifyExistingEmail(string email);
    Task RegisterNewUser(UserEntity user);
}
