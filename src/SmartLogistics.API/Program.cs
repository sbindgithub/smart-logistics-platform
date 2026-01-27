//using Microsoft.EntityFrameworkCore;
//using SmartLogistics.Api.Middleware;
//using SmartLogistics.API.Correlation;
//using SmartLogistics.Application;
//using SmartLogistics.Application.Common;
//using SmartLogistics.Application.Orders;
//using SmartLogistics.Application.Orders.Commands.CreateOrder;
//using SmartLogistics.Domain.Orders.Repositories;
//using SmartLogistics.Infrastructure.Persistence;
//using SmartLogistics.Infrastructure.Persistence.Interceptors;
//using SmartLogistics.Infrastructure.Persistence.Repositories;
//using SmartLogistics.Observability.Common;
//using SmartLogistics.Observability.Tracing;

//using System.Reflection;


//namespace SmartLogistics.API
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//            builder.Services.AddScoped<CreateOrderCommandHandler>();
//            builder.Services.AddHttpContextAccessor();
//            builder.Services.AddScoped<ICorrelationContext, HttpCorrelationContext>();
//            builder.Services.AddApplication();
//            builder.Services.AddScoped<DbCommandPerformanceInterceptor>();
//            builder.Services.AddDbContext<OrdersDbContext>((sp, options) =>
//            {
//                options.UseSqlServer(
//                    builder.Configuration.GetConnectionString("OrdersDb"));

//                options.AddInterceptors(
//                    sp.GetRequiredService<DbCommandPerformanceInterceptor>());
//            });

//            /* ---------- HTTP CLIENT ENFORCEMENT ---------- */
//            builder.Services.AddTransient<CorrelationDelegatingHandler>();

//            builder.Services.AddHttpClient("default")
//                .AddHttpMessageHandler<CorrelationDelegatingHandler>();

//            /* ---------- OPENTELEMETRY WIRING ---------- */
//            builder.Services.AddSmartLogisticsTracing(builder.Configuration);
//            builder.Services.AddScoped<IOrderService, OrderService>();
//            builder.Services.AddScoped(typeof(IAppLogger<>), typeof(CorrelatedLogger<>));

//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();

//            app.UseMiddleware<ExceptionHandlingMiddleware>();
//            app.UseMiddleware<CorrelationIdMiddleware>();
//            app.UseMiddleware<RequestTimingMiddleware>();
//            app.UseMiddleware<RequestPerformanceMiddleware>();
//            app.UseRouting();
//            app.UseAuthorization();
//            app.MapControllers();
//            //app.MapPrometheusScrapingEndpoint();

//            /* ---------- PIPELINE ORDER MATTERS ---------- */
//            app.UseMiddleware<CorrelationMiddleware>();


//            try
//            {
//                app.Run();
//            }
//            catch (ReflectionTypeLoadException ex)
//            {
//                foreach (var loaderException in ex.LoaderExceptions)
//                {
//                    Console.WriteLine(loaderException?.Message);
//                }

//                throw;
//            }

//        }
//    }
//}
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
        builder.Services.AddDbContext<OrdersDbContext>((sp, options) =>
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
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

