using ToDoListApp1.Models.Enums;

namespace ToDoListApp1.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Status IsCompleted { get; set; } = Status.Planned;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime? ReminderDate { get; set; }
    }
}
