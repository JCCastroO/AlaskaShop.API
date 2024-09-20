using AlaskaShop.Shareable.Enums;
using System.ComponentModel.DataAnnotations;

namespace AlaskaShop.Infra.Entities;

public class ProductEntity : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public double Price { get; set; } = default!;
    public string? Description { get; set; } = string.Empty;
    [Required]
    public string Image { get; set; } = string.Empty;
    [Required]
    public ProductTypeEnum Type { get; set; } = default!;
    [Required]
    public Guid CreatedBy { get; set; } = default!;

}
