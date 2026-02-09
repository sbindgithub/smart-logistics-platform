using NetArchTest.Rules;
using SmartLogistics.Domain.Orders;
using SmartLogistics.Domain.Orders.Repositories;
using SmartLogistics.Infrastructure.Persistence;
using Xunit;

namespace SmartLogistics.ArchitectureTests.Architecture;

public class LayerDependencyTests
{
    [Fact]
    public void Domain_Should_Not_Depend_On_Application_Or_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(Order).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "SmartLogistics.Application",
                "SmartLogistics.Infrastructure",
                "Microsoft.EntityFrameworkCore")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Domain layer must not depend on Application, Infrastructure, or EF Core");
    }

    [Fact]
    public void Application_Should_Not_Depend_On_Infrastructure()
    {
        var result = Types
            .InAssembly(typeof(IOrderRepository).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "SmartLogistics.Infrastructure",
                "Microsoft.EntityFrameworkCore")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Application layer must not depend on Infrastructure or EF Core");
    }

    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Presentation_Layer()
    {
        var result = Types
            .InAssembly(typeof(SmartLogisticsDbContext).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "SmartLogistics.API",
                "Microsoft.AspNetCore")
            .GetResult();

        Assert.True(
            result.IsSuccessful,
            "Infrastructure layer must not depend on API or ASP.NET concerns");
    }
}
