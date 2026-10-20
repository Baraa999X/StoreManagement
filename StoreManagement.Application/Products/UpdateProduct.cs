using StoreManagement.Application.Contracts;
using StoreManagement.Application.DTOs;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Products
{
    public class UpdateProduct
    {
        private readonly IProductData _productData;

        public UpdateProduct(IProductData productData)
        {
            _productData = productData;
        }

        public ProductDto? Execute(int id, UpdateProductDto dto)//يعني الميثود لازم ترجع ProductDto أو null
        {
            var product = new Product  //لذلك منعمل نسخة Product من البيانات الموجودة بالـ DTO:
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId
            };//أخذنا البيانات الجديدة من dto وحطيناها بكائن من نوع Product لأن الـ Data Layer بدها Product
            var updatedProduct = _productData.UpdateProduct(id, product);//بعثنا الداتا هون 

            if (updatedProduct == null)
            {
                return null;
            }

            var productDto = new ProductDto//لهيك هون رجعناه
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                CategoryId = updatedProduct.CategoryId
            };

            return productDto;
        }
    }
}