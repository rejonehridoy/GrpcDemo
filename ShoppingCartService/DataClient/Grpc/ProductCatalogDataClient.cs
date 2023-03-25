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

        public async Task<GrpcResponseModel<IEnumerable<Product>>> GetProductListAsync()
        {
            var request = new GrpcProductListRequest { PageNumber = 1, PageSize = 5 };
            var responseModel = new GrpcResponseModel<IEnumerable<Product>>();
            try
            {
                var reply = await client.GetProductListAsync(request);
                if(reply is null)
                {
                    responseModel.ErrorList.Add("Null result is found from grpc server");
                    return responseModel;
                }
                if(reply.TotalCount == 0)
                {
                    responseModel.ErrorList.Add("No Products is found from Catalog");
                    return responseModel;
                }
                responseModel.Data = _mapper.Map<IEnumerable<Product>>(reply.Products);
                responseModel.Success = true;

                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.ErrorList.Add($"Couldnot call GRPC Server {ex.Message}");
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return responseModel;
            }

        }

        public async Task<GrpcResponseModel<Product>> GetProductDetailsByIdAsync(int productId)
        {
            var request = new GrpcProductRequest { ProductId = productId };
            var responseModel = new GrpcResponseModel<Product>();
            try
            {
                var reply = await client.GetProductDetailsAsync(request);
                if(reply is null)
                {
                    responseModel.ErrorList.Add($"No product is found with id: {productId}");
                    return responseModel;
                }
                responseModel.Data = _mapper.Map<Product>(reply);
                responseModel.Success = true;
                return responseModel;
            }
            catch(Exception ex)
            {
                responseModel.ErrorList.Add($"Couldnot call GRPC Server {ex.Message}");
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return responseModel;
            }

        }

        public async Task<GrpcResponseModel<Product>> UpdateProductStockAsync(int productId, int quantity)
        {
            var request = new GrpcProductStockUpdateRequst { ProductId = productId, Quantity = quantity };
            var responseModel = new GrpcResponseModel<Product>();
            try
            {
                var reply = await client.UpdateProuctStockAsync(request);
                if (reply is null)
                {
                    responseModel.ErrorList.Add($"No product is found with id: {productId}");
                    return responseModel;
                }
                responseModel.Data = _mapper.Map<Product>(reply);
                responseModel.Success = true;
                return responseModel;
            }
            catch (Exception ex)
            {
                responseModel.ErrorList.Add($"Couldnot call GRPC Server {ex.Message}");
                Console.WriteLine($"--> Couldnot call GRPC Server {ex.Message}");
                return responseModel;
            }
        }
    }
}
