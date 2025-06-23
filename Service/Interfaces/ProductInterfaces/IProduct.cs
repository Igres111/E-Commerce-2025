using DTOs.ProductDtos;
using Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interfaces.ProductInterfaces
{
    public interface IProduct
    {
        public Task<APIResponse> AddProduct(AddProductDto productInfo);
    }
}
