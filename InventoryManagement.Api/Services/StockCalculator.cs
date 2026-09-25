namespace InventoryManagement.Api.Services;

public static class StockCalculator
{
    public static int? Calculate(int currentQuantity, int change)
    {
        long newQuantity = (long)currentQuantity + change;

        if (newQuantity < 0 || newQuantity > int.MaxValue)
        {
            return null;
        }

        return (int)newQuantity;
    }
}