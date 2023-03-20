using CatalogService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductDetailsById(int productId);
        Task DeleteProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task<Product> CreateProductAsync(Product product);
    }
}
