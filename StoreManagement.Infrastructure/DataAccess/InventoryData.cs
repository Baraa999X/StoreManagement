using Microsoft.EntityFrameworkCore;
using StoreManagement.Application.Interfaces;
using StoreManagement.Domain.Entities;
using StoreManagement.Infrastructure.Data;

namespace StoreManagement.Infrastructure
{
    public class InventoryData : IInventoryData
    {
        private readonly StoreDbContext _context;

        public InventoryData(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<List<Inventory>> GetAllAsync()
        {
            return await _context.Inventories.ToListAsync();//الداتا يلي بقاعده البيانات عم ترجع تروح من خلال العقد وتروح عل منطق
        }

        public async Task<Inventory?> GetByIdAsync(int id)
        {
            return await _context.Inventories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Inventory inventory)
        {
            _context.Inventories.Add(inventory);

            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Inventory inventory)
        {
            _context.Inventories.Update(inventory);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Inventories
                .AnyAsync(i => i.Id == id);
        }
        public async Task DeleteAsync(Inventory inventory)
        {
            _context.Inventories.Remove(inventory);

            await _context.SaveChangesAsync();
        }
    }
}