using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDtos
{
    public class ProductSearchDto
    {
        public string? Query { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public Guid? CategoryId { get; set; }
        public bool? InStock { get; set; }
        public int Page { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
}
