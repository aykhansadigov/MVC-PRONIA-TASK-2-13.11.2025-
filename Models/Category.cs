namespace Backend_MVC_TASK_1.Models
{
    public class Category:BaseEntity
    {
        public string CategoryName { get; set; }
        //relational
        public List<Product> Products { get; set; }
    }
}
