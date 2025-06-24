using DTOs.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common.CategoryResponse
{
    public class GetAllCategoryResponse:APIResponse
    {
        public List<GetAllCategoriesDto> AllCategories { get; set; }
    }
}
