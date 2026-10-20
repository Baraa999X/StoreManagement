using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Application.Categories;
using StoreManagement.Application.DTOs;
using StoreManagement.Application.Products;
using StoreManagement.Application.Results;

namespace StoreManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly GetAllProducts _getAllProducts;
        private readonly GetProductById _getProductById;
        private readonly AddProduct _addProduct;
        private readonly UpdateProduct _updateProduct;
        private readonly DeleteProduct _deleteProduct;

        public ProductsController(GetAllProducts getAllProducts,GetProductById getProductById,AddProduct addProduct,UpdateProduct updateProduct,DeleteProduct deleteProduct)
        {
            _getAllProducts = getAllProducts;
            _getProductById = getProductById;
            _addProduct = addProduct;
            _updateProduct = updateProduct;
            _deleteProduct = deleteProduct;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_getAllProducts.Execute());
        }
        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var product = _getProductById.Execute(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Add(CreateProductDto product)
        {
            var addedProduct = _addProduct.Execute(product);
            return CreatedAtAction(nameof(GetById), new { id = addedProduct.Id }, addedProduct);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProductDto dto)
        {
            var updatedProduct = _updateProduct.Execute(id, dto);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return Ok(updatedProduct);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _deleteProduct.Execute(id);

            if (result.Status == DeleteProductStatus.NotFound)
            {
                return NotFound();
            }

            if (result.Status == DeleteProductStatus.HasOrders)
            {
                return Conflict(new
                {
                    message = "Cannot delete a product that is linked to existing orders."
                });
            }

            return Ok(result.Product);
        }

    }
}