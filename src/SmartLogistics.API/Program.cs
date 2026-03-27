using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Formatting.Compact;
using SmartLogistics.Api.Middleware;
using SmartLogistics.Api.Pipeline;
using SmartLogistics.API.Correlation;
using SmartLogistics.API.Middleware;
using SmartLogistics.Application;
using SmartLogistics.Application.Common;
using SmartLogistics.Infrastructure.DependencyInjection;
using SmartLogistics.Observability.Common;
using SmartLogistics.Observability.Tracing;
using Prometheus;

namespace SmartLogistics.API;

public partial class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(new RenderedCompactJsonFormatter())
    .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog();


        /* ---------- MVC / SWAGGER ---------- */
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        /* ---------- HTTP CONTEXT + CORRELATION ---------- */
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICorrelationContext, HttpCorrelationContext>();

        /* ---------- APPLICATION LAYER ---------- */
        builder.Services.AddApplication();
        builder.Services.AddObservability();

        /* ---------- INFRASTRUCTURE ---------- */
        //builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        //builder.Services.AddScoped<DbCommandPerformanceInterceptor>();
        //builder.Services.AddDbContext<SmartLogisticsDbContext>((sp, options) =>
        //{
        //    options.UseSqlServer(
        //        builder.Configuration.GetConnectionString("OrdersDb"));

        //    options.AddInterceptors(
        //        sp.GetRequiredService<DbCommandPerformanceInterceptor>());
        //});

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
        app.UseHttpMetrics();    // captures HTTP metrics
        app.MapMetrics();        // exposes /metrics

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
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (ctx, http) =>
            {
                ctx.Set("CorrelationId", http.TraceIdentifier);
                ctx.Set("Path", http.Request.Path);
                ctx.Set("Method", http.Request.Method);
            };
        });

        app.MapControllers();
        var cs = builder.Configuration.GetConnectionString("OrdersDb");
        Console.WriteLine($"[DEBUG] OrdersDb = {cs}");

        app.Run();
    }
}

public partial class Program { }

