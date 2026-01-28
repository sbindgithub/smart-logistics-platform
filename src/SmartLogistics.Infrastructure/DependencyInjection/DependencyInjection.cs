using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartLogistics.Application.Orders.Queries.GetOrders;
using SmartLogistics.Infrastructure.Orders;
using SmartLogistics.Infrastructure.Persistence;
using SmartLogistics.Domain.Orders.Repositories;
namespace SmartLogistics.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<SmartLogisticsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("OrdersDb")));

        services.AddScoped<IOrdersReadRepository, OrdersReadRepository>();
        services.AddScoped<IUnitOfWork>(
                     sp => sp.GetRequiredService<SmartLogisticsDbContext>());
        return services;
    }
}
