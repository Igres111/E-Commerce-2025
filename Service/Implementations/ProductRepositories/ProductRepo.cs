using DataAccess.Database;
using DataAccess.Entities;
using DTOs.ProductDtos;
using E_Commerce_2025.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Common.ProductResponses;
using Service.Interfaces.ProductInterfaces;
namespace Service.Implementations.ProductRepositories
{
    public class ProductRepo : IProduct
    {
        public readonly AppDbContext _context;
        private readonly IHubContext<InventoryHub> _hubContext;
        public ProductRepo(AppDbContext context, IHubContext<InventoryHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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
            var categoryIds = productInfo.CategoryId ?? new List<Guid>();

            var categories = await _context.Categories
            .Where(c => categoryIds.Contains(c.Id))
            .ToListAsync();

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
                Categories = categories,
            };
            var broadcast = new ProductBroadcastDto
            {
                Id = newProduct.Id,
                Name = newProduct.Name,
                Description = newProduct.Description,
                Price = newProduct.Price,
                StockQuantity = newProduct.StockQuantity,
                SKU = newProduct.SKU,
                Color = newProduct.Color,
                Size = newProduct.Size,
                CategoryIds = newProduct.Categories.Select(c => c.Id).ToList()
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("ProductAdded", broadcast);
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
                .Include(p => p.Categories)
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

            var broadcast = new ProductBroadcastDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SKU = product.SKU,
                Color = product.Color,
                Size = product.Size,
                CategoryIds = product.Categories.Select(c => c.Id).ToList()
            };
            await _hubContext.Clients.All.SendAsync("ProductUpdated", broadcast);
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
            await _hubContext.Clients.All.SendAsync("ProductDeleted", productId);
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
        public async Task<APIResponse> UpdateVariantPr(UpdateVariantPrDto productInfo)
        {
            var variant = await _context.Products
                .Where(p => p.DeletedAt == null)
                .FirstOrDefaultAsync(p => p.SKU == productInfo.SKU && p.GroupId == productInfo.GroupId);
            if (variant == null)
            {
                return new APIResponse { IsSuccess = false, Error = "Variant not found" };
            }
            variant.Name = string.IsNullOrWhiteSpace(productInfo.Name) ? variant.Name : productInfo.Name;
            variant.Description = string.IsNullOrWhiteSpace(productInfo.Description) ? variant.Description : productInfo.Description;
            variant.Price = productInfo.Price <= 0 ? variant.Price : productInfo.Price;
            variant.StockQuantity = productInfo.StockQuantity < 0 ? variant.StockQuantity : productInfo.StockQuantity;
            variant.SKU = string.IsNullOrWhiteSpace(productInfo.SKU) ? variant.SKU : productInfo.SKU;
            variant.Color = string.IsNullOrWhiteSpace(productInfo.Color) ? variant.Color : productInfo.Color;
            variant.Size = string.IsNullOrWhiteSpace(productInfo.Size) ? variant.Size : productInfo.Size;
            variant.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(variant);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<APIResponse> DeleteVariantPr(DeleteVariationPrDto productInfo)
        {
            var variant = await _context.Products.FirstOrDefaultAsync(p => p.GroupId == productInfo.GroupId && p.SKU == productInfo.SKU);
            if (variant == null)
            {
                return new APIResponse { IsSuccess = false, Error = "Variant not found" };
            }
            variant.DeletedAt = DateTime.UtcNow;
            _context.Products.Update(variant);
            await _context.SaveChangesAsync();
            return new APIResponse { IsSuccess = true };
        }
        public async Task<GetAllItemsResponse> GetAllProducts()
        {
            var items = await _context.Products
                .Where(p => p.DeletedAt == null)
                .OrderBy(p => p.Name)
                .Select(p => new GetProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    SKU = p.SKU,
                    StockQuantity = p.StockQuantity,
                    Color = p.Color,
                    Size = p.Size,
                })
                .ToListAsync();

            return new GetAllItemsResponse
            {
                IsSuccess = true,
                Items = items
            };
        }
        public async Task<GetAllItemsResponse> QuerySearch(ProductSearchDto searchInfo)
        {
            var query = _context.Products
                .Where(p => p.DeletedAt == null);

            if (!string.IsNullOrWhiteSpace(searchInfo.Query))
            {
                query = query.Where(p => p.Name.Contains(searchInfo.Query));
            }

            if (searchInfo.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= searchInfo.MinPrice.Value);
            }
            if (searchInfo.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= searchInfo.MaxPrice.Value);
            }
            if (searchInfo.CategoryId.HasValue)
            {
                query = query.Where(p => p.Categories.Any(c => c.Id == searchInfo.CategoryId.Value));
            }
            if (searchInfo.InStock == true)
            {
                query = query.Where(p => p.StockQuantity > 0);
            }

            query = query.OrderBy(p => p.Name);

            if (searchInfo.PageSize > 0)
            {
                query = query
                    .Skip(searchInfo.Page * searchInfo.PageSize)
                    .Take(searchInfo.PageSize);
            }

            var results = await query
                .Select(q => new GetProductDto
                {
                    Id = q.Id,
                    Name = q.Name,
                    Description = q.Description,
                    Price = q.Price,
                    SKU = q.SKU,
                    StockQuantity = q.StockQuantity,
                    Color = q.Color,
                    Size = q.Size
                })
                .ToListAsync();
            return new GetAllItemsResponse
            {
                IsSuccess = true,
                Items = results
            };
        }
    }
}


