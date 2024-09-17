using AlaskaShop.Infra.Entities;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Infra;

public class Context : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = default!;

    public Context(DbContextOptions<Context> options) : base(options)
    {
    }
}
