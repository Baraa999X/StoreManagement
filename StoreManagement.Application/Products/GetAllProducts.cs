using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;

namespace StoreManagement.Application.Products
{
    public class GetAllProducts
    {
        private readonly IProductData _productData;

        public GetAllProducts(IProductData productData)
        {
            _productData = productData;
        }

        public ICollection<ProductDto> Execute()
        {
            var products = _productData.GetAllProducts(); // عباره عم مجموعة البروداكت الراجعه من العقد 
            var productDtos = products.Select(product => new ProductDto  // برودكت فردي راجع من المجموعه دخلنا علية 
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId
            }).ToList();

            return productDtos;
        }
    }
}