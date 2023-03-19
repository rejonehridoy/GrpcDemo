using ShoppingCartService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartService.DataClient.Grpc
{
    public interface IProductCatalogDataClient
    {
        Task<IEnumerable<Product>> GetProductListAsync();
        Task<Product> GetProductDetailsByIdAsync(int productId);
    }
}
