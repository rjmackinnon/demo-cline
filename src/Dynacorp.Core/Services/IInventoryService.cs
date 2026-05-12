using Dynacorp.Core.Models;

namespace Dynacorp.Core.Services;

/// <summary>
/// Service interface for inventory business logic.
/// </summary>
public interface IInventoryService
{
    IEnumerable<InventoryItem> GetAllInventory();
    InventoryItem? GetInventoryForWidget(int widgetId);
    void AddStock(int widgetId, int quantity);
    void RemoveStock(int widgetId, int quantity);
    IEnumerable<InventoryItem> GetLowStockItems();
    void InitializeWithSampleData();
}