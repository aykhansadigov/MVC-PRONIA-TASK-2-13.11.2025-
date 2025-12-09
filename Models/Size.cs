namespace Backend_MVC_TASK_1.Models
{
    public class Size:BaseEntity
    {
        public string Name { get; set; }
        public List<ProductSize> ProductSizes { get; set; }
    }
}
