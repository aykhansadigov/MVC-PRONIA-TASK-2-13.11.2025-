using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend_MVC_TASK_1.Models
{
    public class Slide:BaseEntity
    {
        [MaxLength(50, ErrorMessage="Max Length 50")]
        [MinLength(2)]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string Image {  get; set; }
        public int Order { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
