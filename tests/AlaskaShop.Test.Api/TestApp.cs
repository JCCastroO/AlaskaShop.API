using AlaskaShop.Infra;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NSubstitute;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace AlaskaShop.Test.Api;

public class TestApp : WebApplicationFactory<Program> 
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        var mediator = Substitute.For<IMediator>();
        var sub = Substitute.For<IAuthenticationSchemeProvider>();
        sub.GetSchemeAsync(Arg.Any<string>()).Returns(new AuthenticationScheme("Bearer", "Bearer", typeof(MockAuthenticationHandler)));
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d
                    => d.ServiceType.Equals(typeof(DbContextOptions<Context>)));
            if (descriptor is not null)
                services.Remove(descriptor);

            var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            services.AddDbContext<Context>(options =>
            {
                options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                options.UseInternalServiceProvider(provider);
            });
        });
    }
}

public class MockAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public MockAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        => Task.FromResult(AuthenticateResult.Success(new(new(new ClaimsIdentity([new("cpf", Guid.NewGuid().ToString())], "Bearer")), "Bearer")));
}