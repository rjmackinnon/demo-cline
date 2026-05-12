using Dynacorp.Core.Models;

namespace Dynacorp.Core.Repositories;

/// <summary>
/// In-memory implementation of the inventory repository.
/// </summary>
public class InMemoryInventoryRepository : IInventoryRepository
{
    private readonly Dictionary<int, InventoryItem> _inventory = new();

    public IEnumerable<InventoryItem> GetAllInventoryItems()
    {
        return _inventory.Values.ToList();
    }

    public InventoryItem? GetInventoryItemByWidgetId(int widgetId)
    {
        return _inventory.TryGetValue(widgetId, out var item) ? item : null;
    }

    public void AddInventoryItem(InventoryItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        _inventory[item.WidgetId] = item;
    }

    public void UpdateInventoryItem(InventoryItem item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));
        if (!_inventory.ContainsKey(item.WidgetId))
            throw new KeyNotFoundException($"Inventory item for widget {item.WidgetId} not found");
        _inventory[item.WidgetId] = item;
    }

    public IEnumerable<InventoryItem> GetLowStockItems()
    {
        return _inventory.Values.Where(item => item.NeedsReorder).ToList();
    }
}