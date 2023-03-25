using ShoppingCartService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartService.DataClient.Grpc
{
    public interface IProductCatalogDataClient
    {
        Task<GrpcResponseModel<IEnumerable<Product>>> GetProductListAsync();
        Task<GrpcResponseModel<Product>> GetProductDetailsByIdAsync(int productId);
        Task<GrpcResponseModel<Product>> UpdateProductStockAsync(int productId, int quantity);
    }
}
