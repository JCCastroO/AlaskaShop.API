using AlaskaShop.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Infra.Repositories.Product.ById;

public class ProductByIdRepository : IProductByIdRepository
{
    private readonly Context _context;

    public ProductByIdRepository(Context context)
        => _context = context;

    public async Task<ProductEntity?> GetProduct(long id)
        => await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
}
