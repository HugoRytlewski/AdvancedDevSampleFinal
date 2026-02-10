using AdvancedDevSample.Domain.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AdvancedDevSample.Api.Tests.Integration
{
    /// <summary>
    /// Custom WebApplicationFactory that replaces dependencies for integration testing.
    /// - Bypasses JWT authentication with a fake handler.
    /// - Uses an in-memory product repository.
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // Remove existing repository registrations
                services.RemoveAll<IProductRepository>();

                // Add in-memory repository as singleton for test isolation
                services.AddSingleton<IProductRepository, InMemoryProductRepository>();

                services.RemoveAll<IClientRepository>();
                services.AddSingleton<IClientRepository, InMemoryClientRepository>();

                services.RemoveAll<IFournisseurRepository>();
                services.AddSingleton<IFournisseurRepository, InMemoryFournisseurRepository>();

                services.RemoveAll<ICommandeRepository>();
                services.AddSingleton<ICommandeRepository, InMemoryCommandeRepository>();

                // Replace authentication with a test scheme that auto-authenticates
                services.AddAuthentication("Test")
                    .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });

                // Set Test as the default scheme
                services.PostConfigure<AuthenticationOptions>(options =>
                {
                    options.DefaultAuthenticateScheme = "Test";
                    options.DefaultChallengeScheme = "Test";
                    options.DefaultScheme = "Test";
                });
            });
        }
    }

    /// <summary>
    /// A fake authentication handler that always succeeds.
    /// Used to bypass JWT authentication in integration tests.
    /// </summary>
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, "TestUser"),
                new Claim(ClaimTypes.NameIdentifier, "test-user-id")
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}