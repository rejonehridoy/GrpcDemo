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
        public ProductService()
        {
            productList = new List<Product>();
            PrepareStaticProductList();
        }

        private void PrepareStaticProductList()
        {
            productList.Add(new Product { Id = 1, Name = "Product_A", Description = "Product A Description", Price = 500, Stock = 5 });
            productList.Add(new Product { Id = 2, Name = "Product_B", Description = "Product B Description", Price = 200, Stock = 7 });
            productList.Add(new Product { Id = 3, Name = "Product_C", Description = "Product C Description", Price = 700, Stock = 11 });
            productList.Add(new Product { Id = 4, Name = "Product_D", Description = "Product D Description", Price = 600, Stock = 22 });
            productList.Add(new Product { Id = 5, Name = "Product_E", Description = "Product E Description", Price = 899, Stock = 5 });
            productList.Add(new Product { Id = 6, Name = "Product_F", Description = "Product F Description", Price = 250, Stock = 11 });
            productList.Add(new Product { Id = 7, Name = "Product_G", Description = "Product G Description", Price = 1199, Stock = 23 });
            productList.Add(new Product { Id = 8, Name = "Product_H", Description = "Product H Description", Price = 2999, Stock = 12 });
            productList.Add(new Product { Id = 9, Name = "Product_I", Description = "Product I Description", Price = 1350, Stock = 16 });
            productList.Add(new Product { Id = 10, Name = "Product_J", Description = "Product J Description", Price = 890, Stock = 6 });
        }
        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await Task.FromResult(productList);
        }

        public async Task<Product> GetProductDetailsById(int productId)
        {
            return await Task.FromResult(productList.FirstOrDefault(product => product.Id == productId));
        }
    }
}
