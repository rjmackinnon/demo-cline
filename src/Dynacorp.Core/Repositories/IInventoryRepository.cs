using Dynacorp.Core.Models;

namespace Dynacorp.Core.Repositories;

/// <summary>
/// Repository interface for inventory data access.
/// </summary>
public interface IInventoryRepository
{
    IEnumerable<InventoryItem> GetAllInventoryItems();
    InventoryItem? GetInventoryItemByWidgetId(int widgetId);
    void AddInventoryItem(InventoryItem item);
    void UpdateInventoryItem(InventoryItem item);
    IEnumerable<InventoryItem> GetLowStockItems();
}