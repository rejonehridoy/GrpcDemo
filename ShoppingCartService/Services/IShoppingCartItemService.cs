using ShoppingCartService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartService.Services
{
    public interface IShoppingCartItemService
    {
        Task<IEnumerable<ShoppingCartItem>> GetAllShoppingCartItemsAsync();
        Task<ShoppingCartItem> GetShoppingCartItemByIdAsync(int id);
        Task<ShoppingCartItem> AddNewShoppingCartItemAsync(ShoppingCartItem shoppingCartItem);
        Task DeleteShoppingCartItemByIdAsync(ShoppingCartItem shoppingCartItem);
        Task UpdateShoppingCartItemAsync(ShoppingCartItem shoppingCartItem);
    }
}
