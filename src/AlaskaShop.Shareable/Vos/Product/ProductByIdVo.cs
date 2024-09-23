namespace AlaskaShop.Shareable.Vos.Product;

public class ProductByIdVo
{
    public long Id { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Price { get; set; } = default!;
    public string Image { get; set; } = string.Empty;
}
