using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.ProductDtos
{
    public class DeleteVariationPrDto
    {
        public Guid GroupId { get; set; }
        public string SKU { get; set; } = string.Empty;
    }
}
