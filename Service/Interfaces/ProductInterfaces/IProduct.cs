using DTOs.ProductDtos;
using Service.Common;
using Service.Common.ProductResponses;

namespace Service.Interfaces.ProductInterfaces
{
    public interface IProduct
    {
        public Task<APIResponse> AddProduct(AddProductDto productInfo);
        public Task<GetProductResponse> GetProduct(Guid productId);
        public Task<APIResponse> UpdateProduct(UpdateProductDto productInfo);
        public Task<APIResponse> DeleteProduct(Guid productId);
        public Task<APIResponse> AddVariantPr(AddVariantPrDto productInfo);
        public Task<GetPrVariantResponse> GetVariantsPr(Guid productId);
        public Task<APIResponse> UpdateVariantPr(UpdateVariantPrDto productInfo);
        public Task<APIResponse> DeleteVariantPr(DeleteVariationPrDto productInfo);
        public Task<GetAllItemsResponse> GetAllProducts();
        public Task<GetAllItemsResponse> QuerySearch(ProductSearchDto searchInfo);
    }
}
