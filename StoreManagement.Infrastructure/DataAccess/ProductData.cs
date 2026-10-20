using Microsoft.EntityFrameworkCore;
using StoreManagement.Application.Contracts;
using StoreManagement.Application.Results;
using StoreManagement.Domain.Entities;
using StoreManagement.Infrastructure.Data;

namespace StoreManagement.Infrastructure.DataAccess
{
    public class ProductData : IProductData
    {
        private readonly StoreDbContext _context;

        public ProductData(StoreDbContext context)
        {
            _context = context;
        }

        public ICollection<Product> GetAllProducts()
        {
            return _context.Products.ToList();
        }

        public Product? GetProductById(int id) => _context.Products.FirstOrDefault(product => product.Id == id);

        public Product AddProduct(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return product;
        }
        public Product? UpdateProduct(int id, Product product)
        {
            var existingProduct = _context.Products.FirstOrDefault(p => p.Id == id);

            if (existingProduct == null)
            {
                return null;
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.CategoryId = product.CategoryId;

            _context.SaveChanges();

            return existingProduct;
        }
        public DeleteProductResult DeleteProduct(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return new DeleteProductResult
                {
                    Status = DeleteProductStatus.NotFound
                };
            }

            var hasOrders = _context.OrderItems.Any(oi => oi.ProductId == id);

            if (hasOrders)
            {
                return new DeleteProductResult
                {
                    Status = DeleteProductStatus.HasOrders
                };
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return new DeleteProductResult
            {
                Status = DeleteProductStatus.Success,
                Product = product
            };
        }


    }
}