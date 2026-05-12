namespace Dynacorp.Core.Models;

/// <summary>
/// Represents an inventory item tracking a widget's stock level.
/// </summary>
public class InventoryItem
{
    public int WidgetId { get; set; }
    public Widget Widget { get; set; } = null!;
    public int QuantityInStock { get; set; }
    public int ReorderLevel { get; set; }

    public bool NeedsReorder => QuantityInStock <= ReorderLevel;

    public void AddStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));
        QuantityInStock += quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("Quantity cannot be negative", nameof(quantity));
        if (quantity > QuantityInStock)
            throw new InvalidOperationException($"Insufficient stock. Available: {QuantityInStock}");
        QuantityInStock -= quantity;
    }
}