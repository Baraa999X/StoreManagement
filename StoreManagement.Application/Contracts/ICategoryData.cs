using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Contracts
{
    public interface ICategoryData
    {
        ICollection<Category> GetAllCategories();
        Category? GetCategoryById(int id);
        Category AddCategory(Category category);
        Category? UpdateCategory(int id, Category category);
        Category? DeleteCategory(int id);
    }
}