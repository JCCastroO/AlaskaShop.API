using System.ComponentModel.DataAnnotations;

namespace AlaskaShop.Infra.Entities;

public class UserEntity : BaseEntity
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public string Password { get; set; } = string.Empty;
    [Required]
    public string Email { get; set; } = string.Empty;
    [Required]
    public bool Active { get; set; } = false;
    public string AccessToken { get; set; } = string.Empty;
}
