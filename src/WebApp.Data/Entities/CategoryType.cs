namespace WebApp.Data.Entities
{
    public class CategoryType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
