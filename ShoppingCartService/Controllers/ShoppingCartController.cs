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
        public async Task<ActionResult<GenericResponseModel<IEnumerable<ShoppingCartItemReadDto>>>> GetAll()
        {
            var model = new GenericResponseModel<IEnumerable<ShoppingCartItemReadDto>>();
            var shoppingCartItems = await _shoppingCartItemService.GetAllShoppingCartItemsAsync();
            if (shoppingCartItems is null && shoppingCartItems.ToList().Count == 0)
            {
                model.ErrorList.Add("No Shopping cart item found");
                return Ok(model);
            }
            model.Data = _mapper.Map<IEnumerable<ShoppingCartItemReadDto>>(shoppingCartItems);
            model.Success = true;
            return Ok(model);
        }

        [HttpGet("id")]
        public async Task<ActionResult<GenericResponseModel<ShoppingCartItemReadDto>>> Get(int id)
        {
            var model = new GenericResponseModel<ShoppingCartItemReadDto>();
            var shoppingCartItem = await _shoppingCartItemService.GetShoppingCartItemByIdAsync(id);
            if (shoppingCartItem == null)
            {
                model.ErrorList.Add($"Shopping cart item is not found of id: {id}");
                return Ok(model);
            }
            model.Data = _mapper.Map<ShoppingCartItemReadDto>(shoppingCartItem);
            model.Success = true;
            return Ok(model);
        }


        [HttpPost]
        public async Task<ActionResult<GenericResponseModel<ShoppingCartItemReadDto>>> AddShoppingCartItem(ShoppingCartItemCreateDto cartItemCreateDto)
        {
            var model = new GenericResponseModel<ShoppingCartItemReadDto>();
            if (cartItemCreateDto.Quantity <= 0)
                return BadRequest("Quantity can be 0 or ngative");
            var newItem = _mapper.Map<ShoppingCartItem>(cartItemCreateDto);
            var responseModel = await _shoppingCartItemService.AddNewShoppingCartItemAsync(newItem);

            if (!responseModel.Success)
            {
                model.ErrorList.AddRange(responseModel.ErrorList);
                return Ok(model);
            }
            model.Data = _mapper.Map<ShoppingCartItemReadDto>(responseModel.Data);
            responseModel.Success = true;
            return Ok(model);
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
        public async Task<ActionResult<GenericResponseModel<ShoppingCartItemReadDto>>> UpdateQuantity(int itemId, int quantity)
        {
            var model = new GenericResponseModel<ShoppingCartItemReadDto>();
            if (quantity <= 0)
                return BadRequest("Quantity can be 0 or ngative");
            var responseModel = await _shoppingCartItemService.UpdateShoppingCartItemAsync(itemId, quantity);
            if (!responseModel.Success)
            {
                model.ErrorList.AddRange(responseModel.ErrorList);
                return Ok(model);
            }
            model.Data = _mapper.Map<ShoppingCartItemReadDto>(responseModel.Data);
            model.Success = true;

            return Ok(model);
        }

        [HttpGet("getProductPrice/{productId}")]
        public async Task<ActionResult<GenericResponseModel<Product>>> GetProductPrice(int productId)
        {
            var responseModel = await _productCatalogDataClient.GetProductDetailsByIdAsync(productId);
            var model = new GenericResponseModel<Product>();
            model.Data = responseModel.Data;
            model.ErrorList = responseModel.ErrorList;
            model.Success = responseModel.Success;
            return Ok(model);
        }

        [HttpGet("getCatalogs")]
        public async Task<ActionResult<GenericResponseModel<IEnumerable<Product>>>> GetCatalogs()
        {
            var model = new GenericResponseModel<IEnumerable<Product>>();
            var grpcResponseModel = await _productCatalogDataClient.GetProductListAsync();
            model.Data = grpcResponseModel.Data;
            model.Success = grpcResponseModel.Success;
            model.ErrorList = grpcResponseModel.ErrorList;
            return Ok(model);
        }
    }
}
