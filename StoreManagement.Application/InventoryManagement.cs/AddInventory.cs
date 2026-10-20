using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.InventoryManagement
{
    public class AddInventory
    {
        private readonly IInventoryData _inventoryData;

        public AddInventory(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        public async Task<Inventory> ExecuteAsync(CreateInventoryDto dto)
        {
            var inventory = new Inventory
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                MinimumStock = dto.MinimumStock,
                LastRestocked = DateTime.UtcNow
            };
            await _inventoryData.AddAsync(inventory);
            return inventory;
        }

    }
}