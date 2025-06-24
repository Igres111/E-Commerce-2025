using DTOs.CategoryDtos;
using Service.Common;
using Service.Common.CategoryResponse;

namespace Service.Interfaces.CategoryInterfaces
{
    public interface ICategory
    {
        public Task<APIResponse> CreateCategory(CreateCategoryDto categoryInfo);
        public Task<GetAllCategoryResponse> GetAllCategories();
        public Task<APIResponse> UpdateCategory(UpdateCategoryDto categoryInfo);
    }
}
