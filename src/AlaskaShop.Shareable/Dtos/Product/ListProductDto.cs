using AlaskaShop.Shareable.Enums;

namespace AlaskaShop.Shareable.Dtos.Product;

public class ListProductDto
{
    public string? Name { get; set; } = string.Empty;
    public ProductTypeEnum? Type { get; set; } = default!;
    public double? MinPrice { get; set; } = default!;
    public double? MaxPrice { get; set; } = default!;
}
