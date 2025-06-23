using DataAccess.Database;
using DataAccess.Entities;
using DTOs.ProductDtos;
using Microsoft.EntityFrameworkCore;
using Service.Common;
using Service.Interfaces.ProductInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Implementations.ProductRepositories
{
    public class ProductRepo:IProduct
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
            return new APIResponse { IsSuccess = true};
            }
    }
}
