using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Backend_MVC_TASK_1.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        //relational
        [ValidateNever]
        public List<Product> Products { get; set; }
    }
}
