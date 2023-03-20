using CatalogService.Data;
using CatalogService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Services
{
    public class ProductService : IProductService
    {
        private List<Product> productList;
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            productList = new List<Product>();
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(await _productRepository.GetAllAsync());
        }

        public async Task<Product> GetProductDetailsById(int productId)
        {
            return await Task.FromResult((await _productRepository.GetAllAsync()).FirstOrDefault(product => product.Id == productId));
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            await _productRepository.InsertAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
            return product;
        }

        public async Task DeleteProductAsync(Product product)
        {
            await _productRepository.RemoveAsync(product);
        }
    }
}
