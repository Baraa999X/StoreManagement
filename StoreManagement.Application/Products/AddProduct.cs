using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Products
{
    public class AddProduct
    {
        private readonly IProductData _productData;

        public AddProduct(IProductData productData)
        {
            _productData = productData;
        }

        public ProductDto Execute(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };

            _productData.AddProduct(product);

            var productDto = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            };

            return productDto;
        }
    }
}