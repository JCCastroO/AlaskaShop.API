using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Response.Auth;
using MediatR;
using OperationResult;

namespace AlaskaShop.Shareable.Request.Auth;

public record RegisterUserRequest(RegisterUserDto Data) : IRequest<Result<RegisterUserResponse>>;
