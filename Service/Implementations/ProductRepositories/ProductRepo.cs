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
            var productExists = await _context.Products
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(p => p.Name == productInfo.Name);

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
                CreatedAt = DateTime.UtcNow,
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<GetProductResponse> GetProduct(Guid productId)
        {
            var product = await _context.Products
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(p => p.Id == productId);

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
            };
        }
        public async Task<APIResponse> UpdateProduct(UpdateProductDto productInfo)
        {
            var product = await _context.Products
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(p => p.Id == productInfo.ProductId);
            if (product == null)
            {
                return new APIResponse { IsSuccess = false, Error = "Product not found" };
            }
            product.Name = string.IsNullOrWhiteSpace(productInfo.Name) ? product.Name : productInfo.Name;
            product.Description = string.IsNullOrWhiteSpace(productInfo.Description) ? product.Description : productInfo.Description;
            product.Price = productInfo.Price <= 0 ? product.Price : productInfo.Price;
            product.StockQuantity = productInfo.StockQuantity < 0 ? product.StockQuantity : productInfo.StockQuantity;
            product.SKU = string.IsNullOrWhiteSpace(productInfo.SKU) ? product.SKU : productInfo.SKU;
            product.Color = string.IsNullOrWhiteSpace(productInfo.Color) ? product.Color : productInfo.Color;
            product.Size = string.IsNullOrWhiteSpace(productInfo.Size) ? product.Size : productInfo.Size;
            product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<APIResponse> DeleteProduct(Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return new APIResponse { IsSuccess = false, Error = "Product not found" };
            }
            product.DeletedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<APIResponse> AddVariantPr(AddVariantPrDto productInfo)
        {
            var product = await _context.Products
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(p => p.Id == productInfo.ProductId || p.GroupId == productInfo.ProductId);
            if (product == null)
            {
                return new APIResponse { IsSuccess = false, Error = "Product not found" };
            }
            var variantExists = await _context.Products
                .Where(v => v.DeletedAt == null)
                .FirstOrDefaultAsync(v => v.SKU == productInfo.SKU && v.GroupId == productInfo.ProductId);
            if (variantExists != null)
            {
                return new APIResponse { IsSuccess = false, Error = "Variant with this SKU already exists" };
            }
            var newVariant = new Product
            {
                Id = Guid.NewGuid(),
                GroupId = product.Id,
                Name = productInfo.Name,
                Description = productInfo.Description,
                Price = productInfo.Price,
                StockQuantity = productInfo.StockQuantity,
                SKU = productInfo.SKU,
                Color = productInfo.Color,
                Size = productInfo.Size,
                CreatedAt = DateTime.UtcNow,
            };
            await _context.Products.AddAsync(newVariant);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<GetPrVariantResponse> GetVariantsPr(Guid productId)
        {
            var products = await _context.Products
                .Where(p => p.DeletedAt == null && (p.Id == productId || p.GroupId == productId))
                .ToListAsync();
            if (products.Count == 0)
            {
                return new GetPrVariantResponse { IsSuccess = false, Error = "Product not found" };
            }
            var newVariants = products.Select(p => new GetPrVariantsDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                SKU = p.SKU,
                Color = p.Color,
                Size = p.Size,
            }).ToList();
            return new GetPrVariantResponse
            {
                IsSuccess = true,
                Variants = newVariants
            };
        }
    }
}


