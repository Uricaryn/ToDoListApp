using System.ComponentModel;
using ToDoListApp1.Models.Enums;

namespace ToDoListApp1.Models.VMs
{
    public class ToDoDeleteVM
    {
        public int Id { get; set; }
        [DisplayName("Task Name")]
        public string Title { get; set; }

        [DisplayName("Status")]
        public Status Status { get; set; }

        [DisplayName("Created Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Description")]
        public string Description { get; set; }
    }
}
