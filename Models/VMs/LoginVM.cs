using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ToDoListApp1.Models.VMs
{
    public class LoginVM
    {
        [Required(ErrorMessage = "Email Field Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Field Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Remember Me")]
        public bool RememberMe { get; set; }
    }
}
