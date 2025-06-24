using DTOs.CategoryDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.CategoryInterfaces;

namespace E_Commerce_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategory _categoryMethods;
        public CategoryController(ICategory categoryMethods)
        {
            _categoryMethods = categoryMethods;
        }
        [HttpPost()]
        public async Task<IActionResult> CreateCategory(CreateCategoryDto categoryInfo)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _categoryMethods.CreateCategory(categoryInfo);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
