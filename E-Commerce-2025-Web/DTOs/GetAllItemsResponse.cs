namespace E_Commerce_2025_Web.DTOs
{
    public class GetAllItemsResponse
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; } = string.Empty;
        public List<Product> Items { get; set; } 
    }
}
