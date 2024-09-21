using AlaskaShop.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Infra.Repositories.Product.List;

public class ListProductRepository : IListProductRepository
{
    private readonly Context _context;

    public ListProductRepository(Context context) => _context = context;

    public async Task<ProductEntity[]> GetProducts()
        => await _context.Products.ToArrayAsync();
}
