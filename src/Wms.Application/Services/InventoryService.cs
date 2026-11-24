using Wms.Application.Interfaces;
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

            ArgumentNullException.ThrowIfNull(slot);
            ArgumentNullException.ThrowIfNull(product);

            await _slotLock.WaitAsync(ct);
            try
            {
                await Task.Delay(100, ct);
                var success = slot.TryPlaceProduct(product);

                return new PlacementResult(success, 
                    $"{product.Sku} {product.Name}" 
                    + (success ? " successfully placed" : " cannot be placed"));

            }
            finally 
            { 
                _slotLock.Release();
            }


        }
    }
}
