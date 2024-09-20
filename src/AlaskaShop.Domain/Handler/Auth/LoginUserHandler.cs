using AlaskaShop.Domain.Services.Crypto;
using AlaskaShop.Domain.Services.Token;
using AlaskaShop.Domain.Services.Validation.Auth;
using AlaskaShop.Infra.Repositories.Auth.Login;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Response.Auth;
using AlaskaShop.Shareable.Vos.Auth;
using MediatR;
using OperationResult;

namespace AlaskaShop.Domain.Handler.Auth;

public class LoginUserHandler : IRequestHandler<LoginUserRequest, Result<LoginUserResponse>>
{
    private readonly ILoginUserRepository _repository;
    private readonly PasswordEncrypter _encrypter;
    private readonly JwtTokenGenerator _token;

    public LoginUserHandler(ILoginUserRepository repository, PasswordEncrypter encrypter, JwtTokenGenerator token)
    {
        _repository = repository;
        _encrypter = encrypter;
        _token = token;
    }

    public async Task<Result<LoginUserResponse>> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var valid = Validate(request.Data);
        if (!valid)
            return new ApplicationException("Request inválido!");

        var password = _encrypter.Encrypt(request.Data.Password);

        var user = await _repository.VerifyExistingUser(request.Data.Email, password);
        if (user is null)
            return new ApplicationException("Usuário não encontrado!");
        if (!user.Active)
            return new ApplicationException("Usuário inativo!");

        var token = _token.Generate(user.UserIdentifier);

        var response = new LoginUserVo()
        {
            Name = user.Name,
            AccessToken = token
        };

        return new LoginUserResponse(response);
    }

    private static bool Validate(LoginUserDto data)
    {
        var validator = new LoginUserValidation();
        var result = validator.Validate(data);

        if (!result.IsValid)
            return false;

        return true;
    }
}
