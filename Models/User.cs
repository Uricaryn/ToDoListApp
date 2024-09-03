using Microsoft.AspNetCore.Identity;

namespace ToDoListApp1.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
