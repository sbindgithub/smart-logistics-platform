using MediatR;
using Microsoft.EntityFrameworkCore;
using SmartLogistics.Api.Middleware;
using SmartLogistics.Api.Pipeline;
using SmartLogistics.API.Correlation;
using SmartLogistics.API.Middleware;
using SmartLogistics.Application;
using SmartLogistics.Application.Common;
using SmartLogistics.Application.Orders;
using SmartLogistics.Domain.Orders.Repositories;
using SmartLogistics.Infrastructure.Persistence;
using SmartLogistics.Infrastructure.Persistence.Interceptors;
using SmartLogistics.Infrastructure.Persistence.Repositories;
using SmartLogistics.Observability.Common;
using SmartLogistics.Observability.Tracing;
using SmartLogistics.Infrastructure.DependencyInjection;

namespace SmartLogistics.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        /* ---------- MVC / SWAGGER ---------- */
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /* ---------- HTTP CONTEXT + CORRELATION ---------- */
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICorrelationContext, HttpCorrelationContext>();

        /* ---------- APPLICATION LAYER ---------- */
        builder.Services.AddApplication();

        /* ---------- INFRASTRUCTURE ---------- */
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        builder.Services.AddScoped<DbCommandPerformanceInterceptor>();
        builder.Services.AddDbContext<SmartLogisticsDbContext>((sp, options) =>
        {
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("OrdersDb"));

            options.AddInterceptors(
                sp.GetRequiredService<DbCommandPerformanceInterceptor>());
        });

        /* ---------- HTTP CLIENT (OUTGOING CORRELATION) ---------- */
        builder.Services.AddTransient<CorrelationDelegatingHandler>();
        builder.Services.AddHttpClient("default")
            .AddHttpMessageHandler<CorrelationDelegatingHandler>();

        /* ---------- OBSERVABILITY ---------- */
        builder.Services.AddSmartLogisticsTracing(builder.Configuration);
        builder.Services.AddScoped(typeof(IAppLogger<>), typeof(CorrelatedLogger<>));
        builder.Services.AddTransient(
            typeof(IPipelineBehavior<,>),
            typeof(ConcurrencyBehavior<,>));

        builder.Services.AddInfrastructure(builder.Configuration);


        var app = builder.Build();

        /* ---------- DEV TOOLS ---------- */
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        /* ---------- MIDDLEWARE PIPELINE (ORDER MATTERS) ---------- */
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<CorrelationMiddleware>();
        app.UseMiddleware<RequestTimingMiddleware>();
        app.UseMiddleware<RequestPerformanceMiddleware>();

        app.UseRouting();
        app.UseMiddleware<GlobalExceptionMiddleware>();

        app.UseAuthorization();

        app.MapControllers();
        var cs = builder.Configuration.GetConnectionString("OrdersDb");
        Console.WriteLine($"[DEBUG] OrdersDb = {cs}");

        app.Run();
    }
}

