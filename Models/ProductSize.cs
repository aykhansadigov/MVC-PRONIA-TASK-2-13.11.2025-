using System.Drawing;

namespace Backend_MVC_TASK_1.Models
{
    public class ProductSize
    {
        public int ProductId { get; set; }
        public int SizeId { get; set; }

        public Product Product { get; set; }
        public Size Size { get; set; }
    }
}
