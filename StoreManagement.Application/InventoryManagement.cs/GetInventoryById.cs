using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.InventoryManagement
{
    public class GetInventoryById
    {
        private readonly IInventoryData _inventoryData;

        public GetInventoryById(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        public async Task<InventoryDto?> ExecuteAsync(int id)
        {
            var inventory = await _inventoryData.GetByIdAsync(id);

            if (inventory == null)
            {
                return null;
            }

            var inventoryDto = new InventoryDto
            {
                Id = inventory.Id,
                ProductId = inventory.ProductId,
                Quantity = inventory.Quantity,
                MinimumStock = inventory.MinimumStock,
                LastRestocked = inventory.LastRestocked
            };

            return inventoryDto;
        }
    }
}
