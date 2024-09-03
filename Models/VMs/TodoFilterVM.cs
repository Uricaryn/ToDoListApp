using Microsoft.AspNetCore.Mvc.Rendering;

namespace ToDoListApp1.Models.VMs
{
    public class TodoFilterVM
    {
        public IEnumerable<ToDoListVM> ToDos { get; set; } // Updated to match your view model
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public string StatusFilter { get; set; }
        public IEnumerable<SelectListItem> Statuses { get; set; } // For filtering
        public string TitleSort { get; set; }
        public string DateSortParam { get; set; }
    }
}
