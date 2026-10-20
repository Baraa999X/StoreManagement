using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Interfaces
{
    public interface IInventoryData
    {
        Task<List<Inventory>> GetAllAsync();
        Task<Inventory?> GetByIdAsync(int id);
        Task AddAsync(Inventory inventory);
        Task UpdateAsync(Inventory inventory);
        Task DeleteAsync(Inventory inventory);
        Task<bool> ExistsAsync(int id);

    }
}