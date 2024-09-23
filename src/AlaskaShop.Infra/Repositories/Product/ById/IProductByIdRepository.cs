using AlaskaShop.Infra.Entities;

namespace AlaskaShop.Infra.Repositories.Product.ById;

public interface IProductByIdRepository
{
    Task<ProductEntity?> GetProduct(long id);
}
