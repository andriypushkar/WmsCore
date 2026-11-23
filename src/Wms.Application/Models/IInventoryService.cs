
using Wms.Domain.Entities;

namespace Wms.Application.Models
{
    public interface IInventoryService
    {
        Task<PlacementResult> PlaceProductInSlotAsync(
            WarehouseSlot slot, Product product, CancellationToken ct = default);
    }
}
