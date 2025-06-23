using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Common
{
    public class APIResponse
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; } = string.Empty;
    }
}
