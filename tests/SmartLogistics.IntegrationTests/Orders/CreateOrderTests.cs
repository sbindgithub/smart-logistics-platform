using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public sealed class CreateOrderTests
    : IClassFixture<TestWebApplicationFactory>
{
    private readonly HttpClient _client;

    public CreateOrderTests(TestWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateOrder_ShouldReturn201()
    {
        // Arrange
        var request = new
        {
            orderNumber = "ORD-0001",
            customerId = Guid.NewGuid(),
            items = new[]
            {
                new
                {
                    productId = Guid.NewGuid(),
                    quantity = 1
                }
            }
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", request);
        var body = await response.Content.ReadAsStringAsync();

        Console.WriteLine("===== RESPONSE BODY =====");
        Console.WriteLine(body);
        Console.WriteLine("=========================");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created, body);
    }

    [Fact]
    public async Task POST_orders_with_missing_required_fields_returns_400()
    {
        // Arrange
        var invalidRequest = new
        {
            orderDate = DateTime.UtcNow
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/orders",
            invalidRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var problem = await response.Content
            .ReadFromJsonAsync<ValidationProblemDetails>();

        problem.Should().NotBeNull();
        problem!.Status.Should().Be(400);
        problem.Errors.Should().NotBeEmpty();
    }
}
