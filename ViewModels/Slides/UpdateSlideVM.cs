using System.ComponentModel.DataAnnotations;

namespace Backend_MVC_TASK_1.ViewModels
{
    public class UpdateSlideVM
    {
        [MaxLength(50, ErrorMessage = "Max Length 50")]
        [MinLength(2)]
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }
        public int Order { get; set; }
        public IFormFile? Photo { get; set; }
    }
}
