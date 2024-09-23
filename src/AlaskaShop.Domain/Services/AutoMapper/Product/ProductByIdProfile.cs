using AlaskaShop.Infra.Entities;
using AlaskaShop.Shareable.Vos.Product;
using AutoMapper;

namespace AlaskaShop.Domain.Services.AutoMapper.Product;

public class ProductByIdProfile : Profile
{
    public ProductByIdProfile()
    {
        CreateMap<ProductEntity, ProductByIdVo>();
    }
}
