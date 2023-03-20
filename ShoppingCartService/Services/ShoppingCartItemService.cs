using ShoppingCartService.Data;
using ShoppingCartService.DataClient.Grpc;
using ShoppingCartService.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartService.Services
{
    public class ShoppingCartItemService : IShoppingCartItemService
    {
        private readonly IProductCatalogDataClient _productCatalogDataClient;
        private readonly IRepository<ShoppingCartItem> _shoppingCartItemRepository;

        public ShoppingCartItemService(IProductCatalogDataClient productCatalogDataClient, IRepository<ShoppingCartItem> shoppingCartItemRepository)
        {
            _productCatalogDataClient = productCatalogDataClient;
            _shoppingCartItemRepository = shoppingCartItemRepository;
        }
        public async Task<ShoppingCartItem> AddNewShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            var product =  await _productCatalogDataClient.GetProductDetailsByIdAsync(shoppingCartItem.ProductId);
            if(product is not null)
            {
                shoppingCartItem.Price = product.Price;
            }
            await _shoppingCartItemRepository.InsertAsync(shoppingCartItem);
            return shoppingCartItem;
        }

        public async Task DeleteShoppingCartItemByIdAsync(ShoppingCartItem shoppingCartItem)
        {
            await _shoppingCartItemRepository.RemoveAsync(shoppingCartItem);
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItemsAsync()
        {
            return await _shoppingCartItemRepository.GetAllAsync();
        }

        public async Task<ShoppingCartItem> GetShoppingCartItemByIdAsync(int id)
        {
            var item = await _shoppingCartItemRepository.GetAsync(id);
            return item;
        }

        public async Task<ShoppingCartItem> UpdateShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            await _shoppingCartItemRepository.UpdateAsync(shoppingCartItem);
            return shoppingCartItem;
        }
    }
}
