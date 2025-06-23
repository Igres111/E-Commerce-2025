using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Category:BaseEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category> Subcategories { get; set; }
        public List<Product> Products { get; set; }
    }
}
