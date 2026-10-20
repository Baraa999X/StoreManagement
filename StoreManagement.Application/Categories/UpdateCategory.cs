using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Categories
{
    public class UpdateCategory
    {
        private readonly ICategoryData _categoryData;

        public UpdateCategory(ICategoryData categoryData)
        {
            _categoryData = categoryData;
        }

        public CategoryDto? Execute(int id, UpdateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            var updatedCategory = _categoryData.UpdateCategory(id, category);

            if (updatedCategory == null)
            {
                return null;
            }

            var categoryDto = new CategoryDto
            {
                Id = updatedCategory.Id,
                Name = updatedCategory.Name
            };

            return categoryDto;
        }
    }
}