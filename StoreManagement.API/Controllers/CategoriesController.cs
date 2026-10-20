using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Categories;
using StoreManagement.Application.DTOs;

namespace StoreManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AddCategory _addCategory;
        private readonly GetAllCategories _getAllCategories;
        private readonly GetCategoryById _getCategoryById;
        private readonly UpdateCategory _updateCategory;
        private readonly DeleteCategory _deleteCategory;
        public CategoriesController(AddCategory addCategory,GetAllCategories getAllCategories,GetCategoryById getCategoryById, UpdateCategory updateCategory, DeleteCategory deleteCategory)
        {
            _addCategory = addCategory;
            _getAllCategories = getAllCategories;
            _getCategoryById = getCategoryById;
            _updateCategory = updateCategory;
            _deleteCategory = deleteCategory;
        }


        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllCategories()
        {
            return Ok(_getAllCategories.Execute());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _getCategoryById.Execute(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(CreateCategoryDto dto)
        {
            var addedCategory = _addCategory.Execute(dto);
            return CreatedAtAction(
                nameof(GetById),
                new { id = addedCategory.Id },
                addedCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCategoryDto dto)
        {
            var updatedCategory = _updateCategory.Execute(id, dto);

            if (updatedCategory == null)
            {
                return NotFound();
            }

            return Ok(updatedCategory);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deletedCategory = _deleteCategory.Execute(id);

            if (deletedCategory == null)
            {
                return NotFound();
            }

            return Ok(deletedCategory);
        }

    }
}