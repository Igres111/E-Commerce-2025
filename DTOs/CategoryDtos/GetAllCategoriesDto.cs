using DataAccess.Entities;

namespace DTOs.CategoryDtos
{
    public class GetAllCategoriesDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<GetSubCategoryDto> Subcategories { get; set; }
    }
}
