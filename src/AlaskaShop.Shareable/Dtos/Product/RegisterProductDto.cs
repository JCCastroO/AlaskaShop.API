using AlaskaShop.Shareable.Enums;

namespace AlaskaShop.Shareable.Dtos.Product;

public class RegisterProductDto
{
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; } = default!;
    public string? Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public ProductTypeEnum Type { get; set; } = default!;
}
