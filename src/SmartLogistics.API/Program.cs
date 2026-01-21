using Microsoft.EntityFrameworkCore;
using SmartLogistics.API.Correlation;
using SmartLogistics.API.Middleware;
using SmartLogistics.Application;
using SmartLogistics.Application.Common;
using SmartLogistics.Application.Orders;
using SmartLogistics.Application.Orders.Commands.CreateOrder;
using SmartLogistics.Domain.Orders.Repositories;
using SmartLogistics.Infrastructure.Persistence;
using SmartLogistics.Infrastructure.Persistence.Interceptors;
using SmartLogistics.Infrastructure.Persistence.Repositories;
//using SmartLogistics.Observability.Common;
//using SmartLogistics.Observability.Tracing;
using System.Reflection;
using SmartLogistics.Api.Middleware;


namespace SmartLogistics.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<CreateOrderCommandHandler>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICorrelationContext, HttpCorrelationContext>();
            builder.Services.AddApplication();
            builder.Services.AddScoped<DbCommandPerformanceInterceptor>();
            builder.Services.AddDbContext<OrdersDbContext>((sp, options) =>
            {
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("OrdersDb"));

                options.AddInterceptors(
                    sp.GetRequiredService<DbCommandPerformanceInterceptor>());
            });

            /* ---------- HTTP CLIENT ENFORCEMENT ---------- */
            builder.Services.AddTransient<CorrelationDelegatingHandler>();

            builder.Services.AddHttpClient("default")
                .AddHttpMessageHandler<CorrelationDelegatingHandler>();

            /* ---------- OPENTELEMETRY WIRING ---------- */
            //builder.Services.AddSmartLogisticsTracing(builder.Configuration);
            //builder.Services.AddScoped(typeof(CorrelatedLogger<>));
            builder.Services.AddScoped<IOrderService, OrderService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<RequestTimingMiddleware>();
            app.UseMiddleware<RequestPerformanceMiddleware>();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            //app.MapPrometheusScrapingEndpoint();

            /* ---------- PIPELINE ORDER MATTERS ---------- */
            app.UseMiddleware<CorrelationMiddleware>();
            

            try
            {
                app.Run();
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    Console.WriteLine(loaderException?.Message);
                }

                throw;
            }

        }
    }
}

