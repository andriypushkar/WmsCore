using Wms.Application.Models;
using Wms.Domain.Entities;

namespace Wms.Application.Interfaces
{
    public interface IInventoryService
    {
        Task<PlacementResult> PlaceProductInSlotAsync(
            WarehouseSlot slot, Product product, CancellationToken ct = default);
    }
}
