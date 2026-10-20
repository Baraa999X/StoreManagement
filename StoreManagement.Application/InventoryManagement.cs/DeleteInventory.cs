using StoreManagement.Application.Interfaces;

namespace StoreManagement.Application.InventoryManagement
{
    public class DeleteInventory
    {
        private readonly IInventoryData _inventoryData;

        public DeleteInventory(IInventoryData inventoryData)
        {
            _inventoryData = inventoryData;
        }

        public async Task<bool> ExecuteAsync(int id)
        {
            var inventory = await _inventoryData.GetByIdAsync(id);

            if (inventory == null)
            {
                return false;
            }

            await _inventoryData.DeleteAsync(inventory);

            return true;
        }
    }
}