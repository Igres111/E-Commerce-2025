using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.ProductInterfaces;

namespace E_Commerce_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProduct _prodMethods;
        public ProductController(IProduct prodMethods)
        {
            _prodMethods = prodMethods;
        }
        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromBody] DTOs.ProductDtos.AddProductDto productInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _prodMethods.AddProduct(productInfo);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(Guid productId)
        {
            var result = await _prodMethods.GetProduct(productId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Product);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProduct([FromBody] DTOs.ProductDtos.UpdateProductDto productInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _prodMethods.UpdateProduct(productInfo);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
    }
}
