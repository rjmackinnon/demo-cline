using Dynacorp.Core.Models;
using Dynacorp.Core.Repositories;
using NLog;

namespace Dynacorp.Core.Services;

/// <summary>
/// Service implementation for inventory business logic.
/// </summary>
public class InventoryService : IInventoryService
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private readonly IInventoryRepository _repository;
    private int _nextWidgetId = 1;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IEnumerable<InventoryItem> GetAllInventory()
    {
        Logger.Info("Retrieving all inventory items");
        return _repository.GetAllInventoryItems();
    }

    public InventoryItem? GetInventoryForWidget(int widgetId)
    {
        Logger.Info($"Retrieving inventory for widget {widgetId}");
        return _repository.GetInventoryItemByWidgetId(widgetId);
    }

    public void AddStock(int widgetId, int quantity)
    {
        Logger.Info($"Adding {quantity} stock to widget {widgetId}");
        var item = _repository.GetInventoryItemByWidgetId(widgetId);
        if (item == null)
            throw new KeyNotFoundException($"Widget {widgetId} not found in inventory");
        
        item.AddStock(quantity);
        _repository.UpdateInventoryItem(item);
        Logger.Info($"New stock level for widget {widgetId}: {item.QuantityInStock}");
    }

    public void RemoveStock(int widgetId, int quantity)
    {
        Logger.Info($"Removing {quantity} stock from widget {widgetId}");
        var item = _repository.GetInventoryItemByWidgetId(widgetId);
        if (item == null)
            throw new KeyNotFoundException($"Widget {widgetId} not found in inventory");
        
        item.RemoveStock(quantity);
        _repository.UpdateInventoryItem(item);
        Logger.Info($"New stock level for widget {widgetId}: {item.QuantityInStock}");
    }

    public IEnumerable<InventoryItem> GetLowStockItems()
    {
        Logger.Info("Retrieving low stock items");
        return _repository.GetLowStockItems();
    }

    public void InitializeWithSampleData()
    {
        Logger.Info("Initializing inventory with sample data");
        
        var sampleWidgets = new[]
        {
            new Widget { Id = _nextWidgetId++, Name = "Iron Lever", Material = Material.Iron, Type = WidgetType.Lever, Price = 29.99m },
            new Widget { Id = _nextWidgetId++, Name = "Copper Wheel", Material = Material.Copper, Type = WidgetType.Wheel, Price = 49.99m },
            new Widget { Id = _nextWidgetId++, Name = "Vibranium Pulley", Material = Material.Vibranium, Type = WidgetType.Pulley, Price = 199.99m },
            new Widget { Id = _nextWidgetId++, Name = "Unobtanium Wedge", Material = Material.Unobtanium, Type = WidgetType.Wedge, Price = 499.99m },
            new Widget { Id = _nextWidgetId++, Name = "Iron Screw", Material = Material.Iron, Type = WidgetType.Screw, Price = 9.99m },
            new Widget { Id = _nextWidgetId++, Name = "Copper Inclined Plane", Material = Material.Copper, Type = WidgetType.InclinedPlane, Price = 79.99m }
        };

        var random = new Random();
        foreach (var widget in sampleWidgets)
        {
            var inventoryItem = new InventoryItem
            {
                WidgetId = widget.Id,
                Widget = widget,
                QuantityInStock = random.Next(10, 100),
                ReorderLevel = 20
            };
            _repository.AddInventoryItem(inventoryItem);
            Logger.Debug($"Added sample inventory item: {widget.Name} (Stock: {inventoryItem.QuantityInStock})");
        }

        // Ensure at least one item is low on stock for demonstration
        var lowStockItem = _repository.GetInventoryItemByWidgetId(1);
        if (lowStockItem != null)
        {
            lowStockItem.QuantityInStock = 5; // Below reorder level of 20
            _repository.UpdateInventoryItem(lowStockItem);
            Logger.Debug($"Set {lowStockItem.Widget.Name} to low stock: {lowStockItem.QuantityInStock}");
        }

        Logger.Info($"Initialized {sampleWidgets.Length} sample inventory items");
    }
}