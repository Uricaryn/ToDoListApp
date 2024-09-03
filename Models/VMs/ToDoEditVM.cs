using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ToDoListApp1.Models.Enums;

namespace ToDoListApp1.Models.VMs
{
    public class ToDoEditVM
    {
        public int Id { get; set; }
        [DisplayName("Task Name")]
        [Required]
        public string Title { get; set; }

        [DisplayName("Status")]
        public Status Status { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("Reminder Date")]
        public DateTime? ReminderDate { get; set; }
    }
}
