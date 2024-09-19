using AlaskaShop.Domain;
using AlaskaShop.Domain.Handler.Auth;
using AlaskaShop.Domain.Services.AutoMapper.Auth;
using AlaskaShop.Domain.Services.Crypto;
using AlaskaShop.Domain.Services.Token;
using AlaskaShop.Infra;
using AlaskaShop.Infra.Repositories.Auth;
using AlaskaShop.Infra.Repositories.Auth.Login;
using AlaskaShop.Infra.Repositories.Auth.Register;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace AlaskaShop.Api.Extensions;

public static class ConfigureServicesExtension
{
    public static void ConfigureServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        ConfigureDatabase(services, configuration);
        ConfigureMediatR(services);
        ConfigureAutoMapper(services);
        ConfigureRepositories(services);
        ConfigureCrypto(services, configuration);
        ConfigureJwtToken(services, configuration);
    }

    private static void ConfigureDatabase(IServiceCollection services, ConfigurationManager configuration)
    {
        string connectionStrings = configuration.GetConnectionString("Default")!;
        services.AddDbContext<Context>(options => options.UseNpgsql(connectionStrings));
    }

    private static void ConfigureMediatR(IServiceCollection services)
        => services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(typeof(RegisterUserHandler).Assembly);
            options.RegisterServicesFromAssemblies(typeof(LoginUserHandler).Assembly);
        });

    private static void ConfigureAutoMapper(IServiceCollection services)
        => services.AddAutoMapper(options =>
        {
            options.AddProfile<RegisterUserProfile>();
        });

    private static void ConfigureRepositories(IServiceCollection services)
    {
        services.AddScoped<IRegisterUserRepository, RegisterUserRepository>();
        services.AddScoped<ILoginUserRepository, LoginUserRepository>();
    }

    private static void ConfigureCrypto(IServiceCollection services, ConfigurationManager configuration)
    {
        var key = configuration.GetValue<string>("Settings:Password:Key");
        services.AddScoped(options => new PasswordEncrypter(key));
    }

    private static void ConfigureJwtToken(IServiceCollection services, ConfigurationManager configuration)
    {
        var key = configuration.GetValue<string>("Settings:JwtToken:Key");
        var expiration = configuration.GetValue<int>("Settings:JwtToken:Expiration");
        services.AddScoped(options => new JwtTokenGenerator(key, expiration));
        services.AddScoped(options => new TokenValidator(key));

        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Description = @"Cabeçalho de autorização JWT usando o esquema Bearer.
                        Insira: 'Bearer' [espaço] e então seu token na entrada de texto abaixo.
                        Exemplo: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme()
                    {
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
    }
}
