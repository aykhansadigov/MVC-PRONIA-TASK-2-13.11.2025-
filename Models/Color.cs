namespace Backend_MVC_TASK_1.Models
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }
        public List<ProductColor> ProductColors { get; set; }
    }

}
