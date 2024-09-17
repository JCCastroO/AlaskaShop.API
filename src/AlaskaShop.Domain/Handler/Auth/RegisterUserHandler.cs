using AlaskaShop.Domain.Services.Validation.Auth;
using AlaskaShop.Infra.Entities;
using AlaskaShop.Infra.Repositories.Auth;
using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Request.Auth;
using AlaskaShop.Shareable.Response.Auth;
using AutoMapper;
using MediatR;

namespace AlaskaShop.Domain.Handler.Auth;

public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    private readonly IRegisterUserRepository _repository;
    private readonly IMapper _mapper;

    public RegisterUserHandler(IRegisterUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var valid = Validate(request.Data);
        if (!valid)
            throw new ApplicationException("Request inválido!");

        var existingEmail = await _repository.VerifyExistingEmail(request.Data.Email);
        if (existingEmail is not null)
            throw new ApplicationException("Usuário já cadastrado!");

        var newUser = _mapper.Map<UserEntity>(request.Data);
        newUser.CreatedAt = new DateOnly();
        newUser.Active = true;

        try
        {
            await _repository.RegisterNewUser(newUser);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
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
