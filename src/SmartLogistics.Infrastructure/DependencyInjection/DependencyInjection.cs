using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartLogistics.Application.Orders.Queries.GetOrders;
using SmartLogistics.Domain.Orders.Repositories;
using SmartLogistics.Infrastructure.Orders;
using SmartLogistics.Infrastructure.Persistence;
using SmartLogistics.Infrastructure.Persistence.Repositories;

namespace SmartLogistics.Infrastructure.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        services.AddDbContext<SmartLogisticsDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("OrdersDb")));

        // MediatR (Domain Events + Handlers)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(SmartLogisticsDbContext).Assembly));

        // Repositories
        services.AddScoped<IOrdersReadRepository, OrdersReadRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        // Unit of Work
        services.AddScoped<IUnitOfWork>(
            sp => sp.GetRequiredService<SmartLogisticsDbContext>());

        return services;
    }
}