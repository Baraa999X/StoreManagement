using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Categories
{
    public class GetCategoryById
    {
        private readonly ICategoryData _categoryData;

        public GetCategoryById(ICategoryData categoryData)
        {
            _categoryData = categoryData;
        }

        public CategoryDto? Execute(int id)
        {
            var category = _categoryData.GetCategoryById(id);

            if (category == null)
            {
                return null;
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            return categoryDto;
        }
    }
}