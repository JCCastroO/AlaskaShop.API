using AlaskaShop.Infra.Entities;
using AlaskaShop.Shareable.Dtos.Auth;
using AutoMapper;

namespace AlaskaShop.Domain.Services.AutoMapper.Auth;

public class RegisterUserProfile : Profile
{
    public RegisterUserProfile()
    {
        CreateMap<RegisterUserDto, UserEntity>();
    }
}
