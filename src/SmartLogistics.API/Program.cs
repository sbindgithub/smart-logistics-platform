using Microsoft.EntityFrameworkCore;
using SmartLogistics.API.Middleware;
using SmartLogistics.Application;
using SmartLogistics.Application.Orders.Commands.CreateOrder;
using SmartLogistics.Domain.Orders.Repositories;
using SmartLogistics.Infrastructure.Observability;
using SmartLogistics.Infrastructure.Persistence;
using SmartLogistics.Infrastructure.Persistence.Repositories;
using System.Reflection;

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

            builder.Services.AddDbContext<OrdersDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("OrdersDb"));
            });

            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<CreateOrderCommandHandler>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ICorrelationContext, HttpCorrelationContext>();
            builder.Services.AddApplication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<CorrelationIdMiddleware>();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RequestPerformanceMiddleware>();

            app.MapControllers();

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

