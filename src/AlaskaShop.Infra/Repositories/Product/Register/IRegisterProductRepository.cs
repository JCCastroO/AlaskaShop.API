using AlaskaShop.Infra.Entities;

namespace AlaskaShop.Infra.Repositories.Product.Register;

public interface IRegisterProductRepository
{
    Task RegisterNewProduct(ProductEntity product);
}
