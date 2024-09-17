using AlaskaShop.Domain;
using AlaskaShop.Domain.Handler.Auth;
using AlaskaShop.Domain.Services.AutoMapper.Auth;
using AlaskaShop.Infra;
using AlaskaShop.Infra.Repositories.Auth;
using Microsoft.EntityFrameworkCore;

namespace AlaskaShop.Api.Extensions;

public static class ConfigureServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        ConfigureDatabase(services, configuration);
        ConfigureMediatR(services);
        ConfigureAutoMapper(services);
        ConfigureRepositories(services);
    }

    private static void ConfigureDatabase(IServiceCollection services, ConfigurationManager configuration)
    {
        string connectionStrings = configuration.GetConnectionString("Default")!;
        services.AddDbContext<Context>(options => options.UseNpgsql(connectionStrings), ServiceLifetime.Scoped);
    }

    private static void ConfigureMediatR(IServiceCollection services)
        => services.AddMediatR(options => options.RegisterServicesFromAssemblies(typeof(RegisterUserHandler).Assembly));

    private static void ConfigureAutoMapper(IServiceCollection services)
        => services.AddAutoMapper(options =>
        {
            options.AddProfile<RegisterUserProfile>();
        });

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();
    }
}
