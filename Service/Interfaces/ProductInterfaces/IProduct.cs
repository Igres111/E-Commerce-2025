using DTOs.ProductDtos;
using Service.Common;
using Service.Common.ProductResponses;

namespace Service.Interfaces.ProductInterfaces
{
    public interface IProduct
    {
        public Task<APIResponse> AddProduct(AddProductDto productInfo);
        public Task<GetProductResponse> GetProduct(Guid productId);
    }
}
