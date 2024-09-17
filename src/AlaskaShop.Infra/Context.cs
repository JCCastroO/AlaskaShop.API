using AlaskaShop.Infra.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace AlaskaShop.Infra;

[ExcludeFromCodeCoverage]
public class Context : DbContext
{
    public DbSet<UserEntity> Users { get; set; } = default!;

    public Context(DbContextOptions<Context> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        => configurationBuilder.Properties<string>().AreUnicode(false).HaveMaxLength(255);
}
