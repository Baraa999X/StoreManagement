using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.InventoryManagement
{
    public class UpdateInventory
    {
        private readonly IInventoryData _inventoryData;

        public UpdateInventory(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        public async Task<bool> ExecuteAsync(int id, UpdateInventoryDto dto)
        {
            var inventory = await _inventoryData.GetByIdAsync(id);

            if (inventory == null)
            {
                return false;
            }

            inventory.Quantity = dto.Quantity;
            inventory.MinimumStock = dto.MinimumStock;
            inventory.LastRestocked = DateTime.UtcNow;

            await _inventoryData.UpdateAsync(inventory);

            return true;
        }
    }
}