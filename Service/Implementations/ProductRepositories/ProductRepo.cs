using DataAccess.Database;
using DataAccess.Entities;
using DTOs.ProductDtos;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Common.ProductResponses;
using Service.Interfaces.ProductInterfaces;

namespace Service.Implementations.ProductRepositories
{
    public class ProductRepo : IProduct
    {
        public readonly AppDbContext _context;
        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse> AddProduct(AddProductDto productInfo)
        {
            var productExists = await _context.Products.FirstOrDefaultAsync(p => p.Name == productInfo.Name);

            if (productExists != null)
            {
                return new APIResponse { IsSuccess = false, Error = "Product already exists" };
            }
            var newProduct = new Product
            {
                Id = Guid.NewGuid(),
                Name = productInfo.Name,
                Description = productInfo.Description,
                Price = productInfo.Price,
                StockQuantity = productInfo.StockQuantity,
                SKU = productInfo.SKU,
                Color = productInfo.Color,
                Size = productInfo.Size,
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<GetProductResponse> GetProduct(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return new GetProductResponse { IsSuccess = false, Error = "Product not found" };
            }
            var productInfo = new GetProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SKU = product.SKU,
                Color = product.Color,
                Size = product.Size
            };

            return new GetProductResponse
            {
                IsSuccess = true,
                Product = productInfo
            }
            ;
        }
    }
}
