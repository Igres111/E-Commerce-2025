using DataAccess.Entities;
using DTOs.CategoryDtos;
using Service.Common;
using Service.Common.CategoryResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.CategoryInterfaces
{
    public interface ICategory
    {
        public Task<APIResponse> CreateCategory(CreateCategoryDto categoryInfo);
        public Task<GetAllCategoryResponse> GetAllCategories();
    }
}
