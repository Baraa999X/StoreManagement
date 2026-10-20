using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Categories
{
    public class DeleteCategory
    {
        private readonly ICategoryData _categoryData;

        public DeleteCategory(ICategoryData categoryData)
        {
            _categoryData = categoryData;
        }

        public CategoryDto? Execute(int id)
        {
            var deletedCategory = _categoryData.DeleteCategory(id);

            if (deletedCategory == null)
            {
                return null;
            }

            var categoryDto = new CategoryDto
            {
                Id = deletedCategory.Id,
                Name = deletedCategory.Name
            };

            return categoryDto;
        }
    }
}