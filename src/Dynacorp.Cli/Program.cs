using Dynacorp.Core.Repositories;
using Dynacorp.Core.Services;
using NLog;

namespace Dynacorp.Cli;

class Program
{
    private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
    private static IInventoryService? _inventoryService;

    static void Main(string[] args)
    {
        try
        {
            // Initialize NLog
            LogManager.Setup().LoadConfigurationFromFile("NLog.config");
            Logger.Info("Dynacorp Inventory System Starting...");

            // Setup dependency injection
            var repository = new InMemoryInventoryRepository();
            _inventoryService = new InventoryService(repository);
            _inventoryService.InitializeWithSampleData();

            // Run the CLI
            RunMenu();
        }
        catch (Exception ex)
        {
            Logger.Fatal(ex, "Application crashed");
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
        finally
        {
            LogManager.Shutdown();
        }
    }

    private static void RunMenu()
    {
        bool exit = false;
        while (!exit)
        {
            Console.Clear();
            Console.WriteLine("=== Dynacorp Inventory System ===");
            Console.WriteLine("1. View All Inventory");
            Console.WriteLine("2. View Low Stock Items");
            Console.WriteLine("3. Add Stock");
            Console.WriteLine("4. Remove Stock");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string? input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    ViewAllInventory();
                    break;
                case "2":
                    ViewLowStock();
                    break;
                case "3":
                    AddStock();
                    break;
                case "4":
                    RemoveStock();
                    break;
                case "5":
                    exit = true;
                    Logger.Info("User exited application");
                    break;
                default:
                    Console.WriteLine("Invalid option. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private static void ViewAllInventory()
    {
        Console.Clear();
        Console.WriteLine("=== All Inventory Items ===");
        var items = _inventoryService!.GetAllInventory();
        foreach (var item in items)
        {
            Console.WriteLine($"ID: {item.WidgetId}, {item.Widget.Name}, Stock: {item.QuantityInStock}, Price: ${item.Widget.Price:F2}{(item.NeedsReorder ? " [LOW STOCK]" : "")}");
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Logger.Info("User viewed all inventory");
    }

    private static void ViewLowStock()
    {
        Console.Clear();
        Console.WriteLine("=== Low Stock Items ===");
        var items = _inventoryService!.GetLowStockItems();
        if (!items.Any())
        {
            Console.WriteLine("No items are low on stock.");
        }
        else
        {
            foreach (var item in items)
            {
                Console.WriteLine($"ID: {item.WidgetId}, {item.Widget.Name}, Stock: {item.QuantityInStock}, Reorder Level: {item.ReorderLevel}");
            }
        }
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Logger.Info("User viewed low stock items");
    }

    private static void AddStock()
    {
        Console.Clear();
        Console.Write("Enter Widget ID: ");
        if (!int.TryParse(Console.ReadLine(), out int widgetId))
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter quantity to add: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            _inventoryService!.AddStock(widgetId, quantity);
            Console.WriteLine($"Added {quantity} to widget {widgetId}. Press any key to continue...");
            Logger.Info($"User added {quantity} stock to widget {widgetId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Press any key to continue...");
            Logger.Warn(ex, $"Failed to add stock to widget {widgetId}");
        }
        Console.ReadKey();
    }

    private static void RemoveStock()
    {
        Console.Clear();
        Console.Write("Enter Widget ID: ");
        if (!int.TryParse(Console.ReadLine(), out int widgetId))
        {
            Console.WriteLine("Invalid ID. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        Console.Write("Enter quantity to remove: ");
        if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity <= 0)
        {
            Console.WriteLine("Invalid quantity. Press any key to continue...");
            Console.ReadKey();
            return;
        }

        try
        {
            _inventoryService!.RemoveStock(widgetId, quantity);
            Console.WriteLine($"Removed {quantity} from widget {widgetId}. Press any key to continue...");
            Logger.Info($"User removed {quantity} stock from widget {widgetId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}. Press any key to continue...");
            Logger.Warn(ex, $"Failed to remove stock from widget {widgetId}");
        }
        Console.ReadKey();
    }
}