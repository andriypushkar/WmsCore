using Wms.Application.Models;
using Wms.Domain.Entities;

namespace Wms.Application.Services
{
    public class InventoryService : IInventoryService
    {

        private readonly SemaphoreSlim _slotLock = new(1, 1);

        public async Task<PlacementResult> PlaceProductInSlotAsync(
            WarehouseSlot slot, Product product, CancellationToken ct = default)
        {
            // Throws OperationCanceledException immediately if cancelled before start
            ct.ThrowIfCancellationRequested();

            await _slotLock.WaitAsync(ct);
            try
            {
                await Task.Delay(100, ct);
                var succes = slot.TryPlaceProduct(product);

                return new PlacementResult(succes, 
                    $"{product.Sku} {product.Name} " 
                    + (succes ? " successfully placed" : " can not be placed"));

            }
            finally 
            { 
                _slotLock.Release();
            }


        }
    }
}
