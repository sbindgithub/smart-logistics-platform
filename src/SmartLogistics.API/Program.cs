using Microsoft.EntityFrameworkCore;
using SmartLogistics.Application.Orders.Abstractions;
using SmartLogistics.Application.Orders.Commands.CreateOrder;
using SmartLogistics.Infrastructure.Persistence;
using SmartLogistics.Infrastructure.Persistence.Repositories;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

