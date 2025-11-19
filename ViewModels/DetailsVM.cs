using Backend_MVC_TASK_1.Models;

namespace Backend_MVC_TASK_1.ViewModels
{
    public class DetailsVM
    {
        public Product Product { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}
