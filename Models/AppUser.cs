using Microsoft.AspNetCore.Identity;

namespace Backend_MVC_TASK_1.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }

    }
}
