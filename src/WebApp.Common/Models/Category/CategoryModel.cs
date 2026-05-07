using WebApp.Common.Enums;

namespace WebApp.Common.Models.Category
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int CategoryTypeId { get; set; }
        public required string CategoryType { get; set; }
    }
}
