using AutoMapper;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using ShoppingCartService.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingCartService.DataClient.Grpc
{
    public class ProductCatalogDataClient : IProductCatalogDataClient
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private ProductCatalog.ProductCatalogClient client; 

        public ProductCatalogDataClient(IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;

            var channel = GrpcChannel.ForAddress(_configuration["GrpcCatalogService"]);
            client = new ProductCatalog.ProductCatalogClient(channel);
        }

        public async Task<IEnumerable<Product>> GetProductListAsync()
        {
            var request = new GrpcProductListRequest { PageNumber = 1, PageSize = 5 };
            try
            {
                var reply = await client.GetProductListAsync(request);
                
                return _mapper.Map<IEnumerable<Product>>(reply.Products);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }

        }

        public async Task<Product> GetProductDetailsByIdAsync(int productId)
        {
            var request = new GrpcProductRequest { ProductId = productId };
            try
            {
                var reply = await client.GetProductDetailsAsync(request);
                return _mapper.Map<Product>(reply);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return null;
            }

        }
    }
}
