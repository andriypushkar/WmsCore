using System.Linq;
using Wms.Application.Models;
using Wms.Application.Services;
using Wms.Domain;
using Wms.Domain.Entities;
using Wms.Domain.Enums;
using Wms.Domain.ValueObjects;

//Module1Tests();
await Module2Tests();

void Module1Tests()
{
    // 1 Defining and initializing list of WarehouseSlots
    List<WarehouseSlot> warehouseSlots = [
        new WarehouseSlot { SlotCode = "ALG-01", MaxWeightKg = 100, MaxVolumeCm3 = 1000 },
        new WarehouseSlot { SlotCode = "ALG-02", MaxWeightKg = 120, MaxVolumeCm3 = 1000 },
        new WarehouseSlot { SlotCode = "ALG-03", MaxWeightKg = 200, MaxVolumeCm3 = 1000 },
        new WarehouseSlot { SlotCode = "ALG-04", MaxWeightKg = 300, MaxVolumeCm3 = 1000 },
    ];

    List<Product> products = [
        new Product { Sku = "T-001", Name = "Tile Type 1", WeightKg = 124, Size = new Dimensions { WidthCm = 10, HeightCm = 10, DepthCm = 10 } },
        new Product { Sku = "T-002", Name = "Tile Type 2", WeightKg = 30, Size = new Dimensions { WidthCm = 2, HeightCm = 2, DepthCm = 2 } },
        new Product { Sku = "T-003", Name = "Tile Type 3", WeightKg = 220, Size = new Dimensions { WidthCm = 3, HeightCm = 3, DepthCm = 3 } },
        new Product { Sku = "T-004", Name = "Tile Type 4", WeightKg = 25, Size = new Dimensions { WidthCm = 4, HeightCm = 4, DepthCm = 4 } },
        new Product { Sku = "T-005", Name = "Tile Type 5", WeightKg = 25, Size = new Dimensions { WidthCm = 5, HeightCm = 5, DepthCm = 5 } },
    ];

    // Let's find all available slots for all products (assume all slots are empty)
    foreach (Product product in products)
    {
        var availSlots = warehouseSlots
            .Where(s => s.CanFit(product))
            .Select(s => new
            {
                Slot = s,
                RemainingWeightKg = s.MaxWeightKg - (s.CurrentWeightKg + product.WeightKg),
            })
            .OrderByDescending(s => s.RemainingWeightKg)
            .ToList();

        System.Console.WriteLine($"For Product: [{product.Sku} {product.Name}], WeightKg: [{product.WeightKg}] are available next Slots: ");
        foreach (var item in availSlots)
        {
            Console.WriteLine($"SlotCode = [{item.Slot.SlotCode}], MaxWeightKg = [{item.Slot.MaxWeightKg}]"
                + $" ,RemainigWeightKg = [{item.RemainingWeightKg}]");
        }
    }
}

async Task Module2Tests()
{
    var service = new InventoryService();

    var slot = new WarehouseSlot { SlotCode = "ALG-100-01", MaxWeightKg = 100, MaxVolumeCm3 = 1000 };

    List<Product> products = new();
    for (var i = 0; i < 5; i++)
    {
        products.Add(new()
        {
            Sku = $"W_{i}",
            Name = $"Prod_{i}",
            WeightKg = 30,
            Size = new Dimensions(10, 10, 0)

        });
    }

    // 1. Run multiple tasks concurrently to test thread safety
    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

    try
    {
        IEnumerable<Task<PlacementResult>> placementTasks = products
            .Select(p => service.PlaceProductInSlotAsync(slot, p, cts.Token));

        PlacementResult[] results = await Task.WhenAll(placementTasks);

        for (var i = 0; i < products.Count; i++)
        {
            Console.WriteLine($"Placement #{i} result = {results[i].Message}");
        }

    }
    catch (OperationCanceledException)
    {
        Console.WriteLine("Operation was canceled or timed out.");
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}


