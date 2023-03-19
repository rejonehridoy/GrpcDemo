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
        public List<ShoppingCartItem> shoppingCartItems = new List<ShoppingCartItem>();
        public ShoppingCartItemService(IProductCatalogDataClient productCatalogDataClient)
        {
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 1, CustomerId = 1, ProductId = 1, Price = 200, quantity = 1 });
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 2, CustomerId = 1, ProductId = 3, Price = 300, quantity = 1 });
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 3, CustomerId = 2, ProductId = 1, Price = 200, quantity = 1 });
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 4, CustomerId = 2, ProductId = 2, Price = 500, quantity = 1 });
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 5, CustomerId = 2, ProductId = 7, Price = 500, quantity = 1 });
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 6, CustomerId = 2, ProductId = 6, Price = 500, quantity = 1 });
            shoppingCartItems.Add(new ShoppingCartItem() { Id = 7, CustomerId = 2, ProductId = 8, Price = 500, quantity = 1 });
            _productCatalogDataClient = productCatalogDataClient;
        }
        public async Task<ShoppingCartItem> AddNewShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            int insertedId = shoppingCartItems.Max(item => item.Id) + 1;
            shoppingCartItem.Id = insertedId;
            var product =  await _productCatalogDataClient.GetProductDetailsByIdAsync(shoppingCartItem.ProductId);
            if(product is not null)
            {
                shoppingCartItem.Price = product.Price;
            }
            shoppingCartItems.Add(shoppingCartItem);
            return shoppingCartItem;
        }

        public Task DeleteShoppingCartItemByIdAsync(ShoppingCartItem shoppingCartItem)
        {
            shoppingCartItems.Remove(shoppingCartItem);
            return Task.CompletedTask;
        }

        public async Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItemsAsync()
        {
            return await Task.FromResult(shoppingCartItems);
        }

        public async Task<ShoppingCartItem> GetShoppingCartItemByIdAsync(int id)
        {
            var item = shoppingCartItems.FirstOrDefault(item => item.Id == id);
            return await Task.FromResult(item);
        }

        public Task UpdateShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            foreach(var item in shoppingCartItems)
            {
                if(item.Id == shoppingCartItem.Id)
                {
                    item.CustomerId = shoppingCartItem.CustomerId;
                    item.ProductId = shoppingCartItem.ProductId;
                    item.quantity = shoppingCartItem.quantity;
                    item.Price = shoppingCartItem.Price;
                    break;
                }
            }
            return Task.CompletedTask;
        }
    }
}
