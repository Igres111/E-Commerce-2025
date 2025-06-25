using E_Commerce_2025_Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_2025_Web.Controllers
{
    public class ProductsController : Controller
    {
        public readonly ProductApiService _productApiService;

        public ProductsController(ProductApiService productApiService)
        {
            _productApiService = productApiService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _productApiService.GetProductsAsync();
            return View(result);
        }
    }
}
