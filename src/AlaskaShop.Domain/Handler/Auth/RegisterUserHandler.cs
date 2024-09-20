using AlaskaShop.Domain.Services.Crypto;
using AlaskaShop.Domain.Services.Validation.Auth;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Auth.Register;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Response.Auth;
using AutoMapper;
using MediatR;
using OperationResult;

namespace AlaskaShop.Domain.Handler.Auth;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, Result<RegisterUserResponse>>
{
    private readonly IRegisterUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly PasswordEncrypter _encrypter;

    public RegisterUserHandler(IRegisterUserRepository repository, IMapper mapper, PasswordEncrypter encrypter)
    {
        _repository = repository;
        _mapper = mapper;
        _encrypter = encrypter;
    }

    public async Task<Result<RegisterUserResponse>> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var valid = Validate(request.Data);
        if (!valid)
            return new ApplicationException("Request inválido!");

        var existingEmail = await _repository.VerifyExistingEmail(request.Data.Email);
        if (existingEmail is not null)
            return new ApplicationException("Usuário já cadastrado!");

        var newUser = _mapper.Map<UserEntity>(request.Data);
        newUser.CreatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        newUser.Active = true;
        newUser.Password = _encrypter.Encrypt(newUser.Password);
        newUser.UserIdentifier = Guid.NewGuid();

        try
        {
            await _repository.RegisterNewUser(newUser);
        }
        catch (Exception)
        {
            return new ApplicationException("Erro ao registrar novo usuário!");
        }

        return new RegisterUserResponse("Cadastro realizado com sucesso!");
    }

    private static bool Validate(RegisterUserDto data)
    {
        var validator = new RegisterUserValidation();
        var result = validator.Validate(data);

        if (!result.IsValid)
            return false;

        return true;
    }
}
