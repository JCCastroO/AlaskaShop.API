using AlaskaShop.Shareable.Dtos.Auth;
using AlaskaShop.Shareable.Response.Auth;
using MediatR;

namespace AlaskaShop.Shareable.Request.Auth;

public record RegisterUserRequest(RegisterUserDto Data) : IRequest<RegisterUserResponse>;
