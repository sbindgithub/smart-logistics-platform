using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmartLogistics.Infrastructure.Persistence;

public sealed class TestWebApplicationFactory
    : WebApplicationFactory<SmartLogistics.API.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<SmartLogisticsDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<SmartLogisticsDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTestsDb"));
        });
    }
}
