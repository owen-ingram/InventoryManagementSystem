using InventoryManagement.Api.Services;
using Xunit;

namespace InventoryManagement.Tests;

public class StockCalculatorTests
{
    [Fact]
    public void Calculate_AddsStockCorrectly()
    {
        int currentQuantity = 10;
        int change = 5;

        int? result = StockCalculator.Calculate(currentQuantity, change);

        Assert.Equal(15, result);
    }

    [Fact]
    public void Calculate_SubtractsStockCorrectly()
    {
        int currentQuantity = 10;
        int change = -3;

        int? result = StockCalculator.Calculate(currentQuantity, change);

        Assert.Equal(7, result);
    }

    [Fact]
    public void Calculate_ReturnsNull_WhenStockWouldBecomeNegative()
    {
        int currentQuantity = 5;
        int change = -10;

        int? result = StockCalculator.Calculate(currentQuantity, change);

        Assert.Null(result);
    }
}