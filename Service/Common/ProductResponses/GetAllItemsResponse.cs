using DTOs.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common.ProductResponses
{
    public class GetAllItemsResponse : APIResponse
    {
        public List<GetProductDto> Items { get; set; } 
    }
}
