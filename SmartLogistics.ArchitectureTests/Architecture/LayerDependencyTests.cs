using FluentAssertions;
using NetArchTest.Rules;
// IMPORTANT:
// Adjust this using to the ACTUAL namespace where IOrderRepository exists
using SmartLogistics.Application;
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

        result.IsSuccessful.Should().BeTrue(
            "Domain must be independent of application and infrastructure");
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

        result.IsSuccessful.Should().BeTrue(
            "Application must not reference infrastructure");
    }

    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Presentation_Layer()
    {
        var result = Types
            .InAssembly(typeof(OrdersDbContext).Assembly)
            .ShouldNot()
            .HaveDependencyOnAny(
                "SmartLogistics.API",
                "Microsoft.AspNetCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(
            "Infrastructure must not depend on API or web concerns");
    }

}
