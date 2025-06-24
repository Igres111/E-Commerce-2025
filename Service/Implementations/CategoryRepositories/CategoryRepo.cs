using DataAccess.Database;
using DataAccess.Entities;
using DTOs.CategoryDtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.Common;
using Service.Common.CategoryResponse;
using Service.Interfaces.CategoryInterfaces;

namespace Service.Implementations.CategoryRepositories
{
    public class CategoryRepo : ICategory
    {
        public readonly AppDbContext _context;
        public CategoryRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<APIResponse> CreateCategory(CreateCategoryDto categoryInfo)
        {
            if (categoryInfo.Name.IsNullOrEmpty() || categoryInfo.ParentCategoryId == Guid.Empty)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Error = "Wrong Credentials",
                };
            }
            var categoryExists = await _context.Categories
                .AnyAsync(c => c.Name == categoryInfo.Name && c.ParentCategoryId == categoryInfo.ParentCategoryId);
            if (categoryExists)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Error = "Category already exists",
                };
            }
            var category = new Category
            {
                Name = categoryInfo.Name,
                ParentCategoryId = categoryInfo.ParentCategoryId,
                CreatedAt = DateTime.UtcNow
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return new APIResponse
            {
                IsSuccess = true
            };
        }
        public async Task<GetAllCategoryResponse> GetAllCategories()
        {
            var categories = await _context.Categories
                .Include(c => c.Subcategories)
                .ToListAsync();

            if (categories.Count == 0)
            {
                return new GetAllCategoryResponse
                {
                    IsSuccess = false,
                    Error = "No categories found"
                };
            }

            var result = categories
                .Where(c => c.ParentCategoryId == null)
                .Select(c => new GetAllCategoriesDto
            {
                Name = c.Name,
                Id = c.Id,
                Subcategories = c.Subcategories.Select(sub => new GetSubCategoryDto
                {
                    Id = sub.Id,
                    Name = sub.Name,
                }).ToList()
            }).ToList();

            return new GetAllCategoryResponse { IsSuccess = true, AllCategories = result };
        }
    }
}