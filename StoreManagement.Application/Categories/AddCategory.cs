using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Categories
{
    public class AddCategory
    {
        private readonly ICategoryData _categoryData;

        public AddCategory(ICategoryData categoryData)
        {
            _categoryData = categoryData;
        }
        public CategoryDto Execute(CreateCategoryDto dto)//بفوت بهي الجسم 
        {
            var Category = new Category
            {
                Name = dto.Name,
            };

            _categoryData.AddCategory(Category);
            var categoryDto = new CategoryDto
            {
                Id = Category.Id,
                Name=Category.Name
            };

            return categoryDto;
        }



    }
}
