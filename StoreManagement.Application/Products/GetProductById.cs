using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Products
{
    public class GetProductById
    {
        private readonly IProductData _productData;

        public GetProductById(IProductData productData)
        {
            _productData = productData;
        }

        public ProductDto? Execute(int id)
        {
            var product = _productData.GetProductById(id);

            if (product == null)
            {
                return null;
            }

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