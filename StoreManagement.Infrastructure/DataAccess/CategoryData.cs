using StoreManagement.Application.Contracts;
using StoreManagement.Domain.Entities;
using StoreManagement.Infrastructure.Data;

namespace StoreManagement.Infrastructure.DataAccess
{
    public class CategoryData : ICategoryData
    {
        private readonly StoreDbContext _context;

        public CategoryData(StoreDbContext context)
        {
            _context = context;
        }
        public Category AddCategory(Category category)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return category;
        }
        public ICollection<Category> GetAllCategories()
        {
            return _context.Categories.ToList();
        }
        public Category? GetCategoryById(int id) => _context.Categories.FirstOrDefault(category => category.Id == id);
        public Category? UpdateCategory(int id, Category category)
        {
            var existingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;

            _context.SaveChanges();

            return existingCategory;
        }
        public Category? DeleteCategory(int id)
        {
            var deletingCategory = _context.Categories.FirstOrDefault(c => c.Id == id);

            if (deletingCategory == null)
            {
                return null;
            }

            _context.Categories.Remove(deletingCategory);
            _context.SaveChanges();

            return deletingCategory;
        }
    }
}