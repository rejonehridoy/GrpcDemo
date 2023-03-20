using AutoMapper;
using CatalogService.Dtos;
using CatalogService.Models;
using CatalogService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAll()
        {
            var products = await _productService.GetAllProductsAsync();
            if (products is null && products.ToList().Count == 0)
                NotFound("No product is found");
            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }

        [HttpGet("id")]
        public async Task<ActionResult<ProductReadDto>> Get(int id)
        {
            var product = await _productService.GetProductDetailsById(id);
            if (product == null)
                return NotFound($"Product with id: {id} is not found in the database");

            return Ok(_mapper.Map<ProductReadDto>(product));
        }

        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> AddShoppingCartItem(ProductCreateDto productCreateDto)
        {
            var newItem = _mapper.Map<Product>(productCreateDto);
            var insertedProduct = await _productService.CreateProductAsync(newItem);

            return Ok(_mapper.Map<ProductReadDto>(insertedProduct));
        }

        [HttpDelete("productId")]
        public async Task<ActionResult> DeleteShoppingCartItem(int productId)
        {
            var product = await _productService.GetProductDetailsById(productId);
            if (product is null)
                NotFound($"No product is found to delete of id: {productId}");
            await _productService.DeleteProductAsync(product);
            return Ok($"product Deleted with id: {productId}");
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductReadDto>> UpdateQuantity(int productId, ProductUpdateDto productUpdateDto)
        {
            //var product = await _productService.GetProductDetailsById(productId);
            //if (product is null)
            //    NotFound($"Product is not found of id: {productId}");

            var updatedProduct = _mapper.Map<Product>(productUpdateDto);
            updatedProduct.Id = productId;
            await _productService.UpdateProductAsync(updatedProduct);
            return Ok(_mapper.Map<ProductReadDto>(updatedProduct));
        }
    }
}
