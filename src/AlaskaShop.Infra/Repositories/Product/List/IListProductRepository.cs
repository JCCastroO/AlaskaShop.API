using AlaskaShop.Infra.Entities;

namespace AlaskaShop.Infra.Repositories.Product.List;

public interface IListProductRepository
{
    Task<ProductEntity[]> GetProducts();
}
