using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Categories
{
    public class GetAllCategories
    {
        private readonly ICategoryData _categoryData;

        public GetAllCategories(ICategoryData categoryData)
        {
            _categoryData = categoryData;
        }

        public ICollection<CategoryDto> Execute()
        {
            var categories = _categoryData.GetAllCategories();

            var categoryDtos = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();

            return categoryDtos;
        }
    }
}