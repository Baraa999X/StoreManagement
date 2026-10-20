using StoreManagement.Application.Results;
using StoreManagement.Domain.Entities;

namespace StoreManagement.Application.Contracts
{
    public interface IProductData
    {
        ICollection<Product> GetAllProducts();

        Product? GetProductById(int id);
        Product AddProduct(Product product);
        Product? UpdateProduct(int id, Product product);
        DeleteProductResult DeleteProduct(int id);
    }
}