namespace Backend_MVC_TASK_1.Models
{
    public class ProductImage:BaseEntity
    {
        public string Image { get; set; }
        public bool? IsPrimaryImage { get; set; }
        //relational
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
