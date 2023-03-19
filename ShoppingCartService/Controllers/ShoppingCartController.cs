using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.DataClient.Grpc;
using ShoppingCartService.Dtos;
using ShoppingCartService.Models;
using ShoppingCartService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCartService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductCatalogDataClient _productCatalogDataClient;
        private readonly IShoppingCartItemService _shoppingCartItemService;

        public ShoppingCartController(IMapper mapper, IProductCatalogDataClient productCatalogDataClient, IShoppingCartItemService shoppingCartItemService)
        {
            _mapper = mapper;
            _productCatalogDataClient = productCatalogDataClient;
            _shoppingCartItemService = shoppingCartItemService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShoppingCartItemReadDto>>> GetAll()
        {
            var shoppingCartItems = await _shoppingCartItemService.GetAllShoppingCartItemsAsync();
            if (shoppingCartItems is null && shoppingCartItems.ToList().Count == 0)
                NotFound("No Shopping cart item found");
            return Ok(_mapper.Map<IEnumerable<ShoppingCartItemReadDto>>(shoppingCartItems));
        }

        [HttpGet("id")]
        public async Task<ActionResult<ShoppingCartItemReadDto>> Get(int id)
        {
            var shoppingCartItem = await _shoppingCartItemService.GetShoppingCartItemByIdAsync(id);
            if (shoppingCartItem == null)
                return NotFound();

            return Ok(_mapper.Map<ShoppingCartItemReadDto>(shoppingCartItem));
        }


        [HttpPost]
        public async Task<ActionResult<ShoppingCartItemReadDto>> AddShoppingCartItem(ShoppingCartItemCreateDto cartItemCreateDto)
        {
            var newItem = _mapper.Map<ShoppingCartItem>(cartItemCreateDto);
            var insertedItem = await _shoppingCartItemService.AddNewShoppingCartItemAsync(newItem);

            return Ok(_mapper.Map<ShoppingCartItemReadDto>(insertedItem));
        }

        [HttpDelete("itemId")]
        public async Task<ActionResult> DeleteShoppingCartItem(int itemId)
        {
            var item = await _shoppingCartItemService.GetShoppingCartItemByIdAsync(itemId);
            if (item is null)
                NotFound($"No shopping cart item found to delete of id: {itemId}");
            await _shoppingCartItemService.DeleteShoppingCartItemByIdAsync(item);
            return Ok($"Shopping Item Deleted with id: {itemId}");
        }

        [HttpPut("{itemId}/{quantity}")]
        public async Task<ActionResult<ShoppingCartItemReadDto>> UpdateQuantity(int itemId, int quantity)
        {
            var item = await _shoppingCartItemService.GetShoppingCartItemByIdAsync(itemId);
            if (item is null)
                NotFound($"Shopping cart item is not found of id: {itemId}");
            item.quantity = quantity;
            await _shoppingCartItemService.UpdateShoppingCartItemAsync(item);
            return Ok(_mapper.Map<ShoppingCartItemReadDto>(item));
        }

        [HttpGet("getProductPrice/{productId}")]
        public async Task<ActionResult<Product>> GetProductPrice(int productId)
        {
            var product = await _productCatalogDataClient.GetProductDetailsByIdAsync(productId);
            if (product is null)
                return NotFound($"No Product is found of id: {productId}");
            return Ok(product);
        }

        [HttpGet("getCatalogs")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCatalogs()
        {
            var productList = (await _productCatalogDataClient.GetProductListAsync()).ToList();
            if (productList is null || productList.Count <= 0)
                return NotFound();
            return Ok(productList);
        }
    }
}
