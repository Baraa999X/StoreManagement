using StoreManagement.Application.Contracts;
using StoreManagement.Application.Results;

namespace StoreManagement.Application.Products
{
    public class DeleteProduct
    {
        private readonly IProductData _productData;

        public DeleteProduct(IProductData productData)
        {
            _productData = productData;
        }
        public DeleteProductResult Execute(int id)
        {
            return _productData.DeleteProduct(id);
        }
    }
}
