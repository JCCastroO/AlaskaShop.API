using AlaskaShop.Infra.Entities;

namespace AlaskaShop.Infra.Repositories.Product.Register;

public class RegisterProductRepository : IRegisterProductRepository
{
    private readonly Context _context;

    public RegisterProductRepository(Context context)
        => _context = context;

    public async Task RegisterNewProduct(ProductEntity product)
    {
        _context.Products.Add(product);
        await _context.SaveChangesAsync();
    }
}
