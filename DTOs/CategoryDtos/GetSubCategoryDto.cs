﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.CategoryDtos
{
    public class GetSubCategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
