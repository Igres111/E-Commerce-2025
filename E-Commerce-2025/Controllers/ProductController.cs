using DTOs.ProductDtos;
using E_Commerce_2025.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces.ProductInterfaces;

namespace E_Commerce_2025.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public readonly IProduct _prodMethods;
        private readonly IHubContext<InventoryHub> _hubContext;

        public ProductController(IProduct prodMethods, IHubContext<InventoryHub> hubContext)
        {
            _prodMethods = prodMethods;
            _hubContext = hubContext;
        }
        [HttpPost()]
        public async Task<IActionResult> AddProduct(AddProductDto productInfo)
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

        [HttpPut()]
        public async Task<IActionResult> UpdateProduct(UpdateProductDto productInfo)
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
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var result = await _prodMethods.DeleteProduct(productId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result);
        }

        [HttpPost("{productId}/variants")]
        public async Task<IActionResult> AddVariant(AddVariantPrDto productInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _prodMethods.AddVariantPr(productInfo);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }

        [HttpGet("{productId}/variants")]
        public async Task<IActionResult> GetVariant(Guid productId)
        {
            var result = await _prodMethods.GetVariantsPr(productId);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result.Variants);
        }

        [HttpPut("variants")]
        public async Task<IActionResult> UpdateVariant(UpdateVariantPrDto productInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _prodMethods.UpdateVariantPr(productInfo);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }
            return Ok(result);
        }
        [HttpDelete("variants")]
        public async Task<IActionResult> DeleteVariant(DeleteVariationPrDto productInfo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _prodMethods.DeleteVariantPr(productInfo);
            if (!result.IsSuccess)
            {
                return NotFound(result.Error);
            }
            return Ok(result);
        }
        [HttpGet("Get-All")]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _prodMethods.GetAllProducts();
            return Ok(result);
        }
    }
}
