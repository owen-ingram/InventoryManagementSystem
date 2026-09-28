using System.Net;
using System.Net.Http.Json;
using InventoryManagement.Api.Models;

namespace InventoryManagement.Tests;

public class ProductsApiTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ProductsApiTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateProduct_ReturnsCreatedProduct()
    {
        var request = new ProductRequest
        {
            Name = "Test Keyboard",
            Description = "Integration test keyboard",
            Price = 49.99m,
            Quantity = 10
        };

        var response = await _client.PostAsJsonAsync(
            "/api/products",
            request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var product =
            await response.Content.ReadFromJsonAsync<Product>();

        Assert.NotNull(product);
        Assert.Equal("Test Keyboard", product.Name);
        Assert.Equal(10, product.Quantity);
        Assert.True(product.Id > 0);
    }

    [Fact]
    public async Task CreateProduct_WithInvalidData_ReturnsBadRequest()
    {
        var request = new ProductRequest
        {
            Name = "",
            Description = "Invalid product",
            Price = -10,
            Quantity = -5
        };

        var response = await _client.PostAsJsonAsync(
            "/api/products",
            request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task AdjustStock_UpdatesProductQuantity()
    {
        var createRequest = new ProductRequest
        {
            Name = "Test Mouse",
            Description = "Mouse for stock test",
            Price = 29.99m,
            Quantity = 10
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/products",
            createRequest);

        var product =
            await createResponse.Content.ReadFromJsonAsync<Product>();

        Assert.NotNull(product);

        var stockRequest = new
        {
            change = 5
        };

        var response = await _client.PatchAsJsonAsync(
            $"/api/products/{product.Id}/stock",
            stockRequest);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var updatedProduct =
            await response.Content.ReadFromJsonAsync<Product>();

        Assert.NotNull(updatedProduct);
        Assert.Equal(15, updatedProduct.Quantity);
    }

    [Fact]
    public async Task GetLowStockProducts_ReturnsOnlyProductsBelowThreshold()
    {
        var lowStockProduct = new ProductRequest
        {
            Name = "Low Stock Item",
            Description = "Should be returned",
            Price = 19.99m,
            Quantity = 3
        };

        var highStockProduct = new ProductRequest
        {
            Name = "High Stock Item",
            Description = "Should not be returned",
            Price = 29.99m,
            Quantity = 20
        };

        await _client.PostAsJsonAsync("/api/products", lowStockProduct);
        await _client.PostAsJsonAsync("/api/products", highStockProduct);

        var response = await _client.GetAsync(
            "/api/products/low-stock?threshold=5");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var products =
            await response.Content.ReadFromJsonAsync<List<Product>>();

        Assert.NotNull(products);

        Assert.Contains(products, p => p.Name == "Low Stock Item");
        Assert.DoesNotContain(products, p => p.Name == "High Stock Item");
    }

    [Fact]
    public async Task GetProduct_WithInvalidId_ReturnsNotFound()
    {
        var response = await _client.GetAsync(
            "/api/products/999999");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}