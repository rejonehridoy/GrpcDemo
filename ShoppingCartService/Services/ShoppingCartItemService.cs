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
        public async Task<GenericResponseModel<ShoppingCartItem>> AddNewShoppingCartItemAsync(ShoppingCartItem shoppingCartItem)
        {
            var responseModel = new GenericResponseModel<ShoppingCartItem>();
            var grpcResponseModel =  await _productCatalogDataClient.GetProductDetailsByIdAsync(shoppingCartItem.ProductId);

            if (!grpcResponseModel.Success)
            {
                responseModel.ErrorList.AddRange(grpcResponseModel.ErrorList);
                return responseModel;
            }
            var product = grpcResponseModel.Data;

            if(product is not null)
            {
                if (product.Stock - shoppingCartItem.quantity < 0)
                {
                    responseModel.ErrorList.Add("Not enough stock");
                    return responseModel;
                }
                shoppingCartItem.Price = product.Price;
                grpcResponseModel = await _productCatalogDataClient.UpdateProductStockAsync(shoppingCartItem.ProductId, shoppingCartItem.quantity);
                if (!grpcResponseModel.Success)
                {
                    responseModel.ErrorList.AddRange(grpcResponseModel.ErrorList);
                    return responseModel;
                }
                await _shoppingCartItemRepository.InsertAsync(shoppingCartItem);
                responseModel.Data = shoppingCartItem;
                responseModel.Success = true;
                return responseModel;
            }
            responseModel.ErrorList.Add("Product is null");
            return responseModel;
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

        public async Task<GenericResponseModel<ShoppingCartItem>> UpdateShoppingCartItemAsync(int itemId, int Quanttiy)
        {
            var responseModel = new GenericResponseModel<ShoppingCartItem>();
            var shoppingCartItem = await GetShoppingCartItemByIdAsync(itemId);
            if(shoppingCartItem is not null)
            {
                var newQuantity = Quanttiy - shoppingCartItem.quantity;
                if(newQuantity != 0)
                {
                    var grpcResponseModel = await _productCatalogDataClient.UpdateProductStockAsync(shoppingCartItem.ProductId, newQuantity);
                    if (!grpcResponseModel.Success)
                    {
                        responseModel.ErrorList.AddRange(grpcResponseModel.ErrorList);
                        return responseModel;
                    }
                }

            }
            else
            {
                responseModel.ErrorList.Add("$No shopping cart item is found of id: {itemId}");
                return responseModel;
            }
            shoppingCartItem.quantity = Quanttiy;
            await _shoppingCartItemRepository.UpdateAsync(shoppingCartItem);
            responseModel.Success = true;
            responseModel.Data = shoppingCartItem;
            return responseModel;
        }
    }
}
