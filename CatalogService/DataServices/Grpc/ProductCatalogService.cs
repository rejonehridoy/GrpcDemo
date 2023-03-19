using AutoMapper;
using CatalogService.Services;
using Grpc.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.DataServices.Grpc
{
    public class ProductCatalogService : ProductCatalog.ProductCatalogBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductCatalogService(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }
        public override async Task<GrpcProductList> GetProductList(GrpcProductListRequest request, ServerCallContext context)
        {
            var productLists = await _productService.GetAllProductsAsync();
            if(productLists is null)
                return null;

            var response = new GrpcProductList();
            response.Products.AddRange(_mapper.Map<IEnumerable<GrpcProductModel>>(productLists));
            response.TotalCount = productLists.ToList().Count;
            return await Task.FromResult(response);
        }

        public override async Task<GrpcProductModel> GetProductDetails(GrpcProductRequest request, ServerCallContext context)
        {
            var product = await _productService.GetProductDetailsById(request.ProductId);
            if (product is null)
                return null;
            return _mapper.Map<GrpcProductModel>(product);
        }
    }
}
