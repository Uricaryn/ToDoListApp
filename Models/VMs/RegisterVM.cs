using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ToDoListApp1.Models.VMs
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "Name Field Required")]
        [DisplayName("Your Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Surname Field Required.")]
        [DisplayName("Surname")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email Field Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DisplayName("Password")]
        [Required(ErrorMessage = "Password Field Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Passwords Does Not Match")]
        [DataType(dataType: DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
