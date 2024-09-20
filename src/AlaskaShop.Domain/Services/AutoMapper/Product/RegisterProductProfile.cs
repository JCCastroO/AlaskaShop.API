using AlaskaShop.Infra.Entities;
using AlaskaShop.Shareable.Dtos.Product;
using AutoMapper;

namespace AlaskaShop.Domain.Services.AutoMapper.Product;

public class RegisterProductProfile : Profile
{
    public RegisterProductProfile()
    {
        CreateMap<RegisterProductDto, ProductEntity>();
    }
}
