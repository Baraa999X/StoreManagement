using StoreManagement.Application.DTOs;
using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.InventoryManagement
{
    public class GetAllInventory
    {
        private readonly IInventoryData _inventoryData;

        public GetAllInventory(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        public async Task<List<InventoryDto>> ExecuteAsync()
        {
            var inventories = await _inventoryData.GetAllAsync();//هون الداتا المحمله الراجعه من العقد من خلال الداتا اكسس
            // هون المنطق تخزنت الداتا الراجعه بالمتحول
            var inventoryDtos = inventories.Select(i => new InventoryDto//هون بكل بساطه فوت عل داتا الراجعه وعميل اوبجكت وحول لداتا 
            {
                Id = i.Id,
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                MinimumStock = i.MinimumStock,
                LastRestocked = i.LastRestocked
            }).ToList();

            return inventoryDtos;
        }
    }
}