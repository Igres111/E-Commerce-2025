﻿using E_Commerce_2025_Web.DTOs;

namespace E_Commerce_2025_Web.Services
{
    public class ProductApiService
    {
        private readonly HttpClient _httpClient;
        public ProductApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("E-commerce-2025");
        }
        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("/api/Product/Get-All");
            if (!response.IsSuccessStatusCode)
                throw new Exception("Failed to load products from API");

            var wrapper = await response.Content
                .ReadFromJsonAsync<GetAllItemsResponse>();

            if (wrapper == null || !wrapper.IsSuccess)
                throw new Exception(wrapper?.Error ?? "Empty response");

            return wrapper.Items;
        }
    }
}
