using ToDoListApp1.Models;
using ToDoListApp1.Models.Enums;
using ToDoListApp1.Models.VMs;

namespace ToDoListApp1.Services
{
    public interface ITodoService
    {
        bool AddToDo(ToDoCreateVM toDo, string userId);
        Task<bool> UpdateToDoAsync(ToDoEditVM toDoEditVM, string userId);
        Task<bool> DeleteToDoAsync(int id, string userId);
        Task<bool> TodoExistAsync(int id);
        bool Delete(int id);
        Task<ToDoItem> GetToDoByIdAsync(int id);
        Task<List<ToDoItem>> GetToDoByNameAsync(string toDoTitle);
        Task<List<ToDoItem>> GetToDoByStatus(Status status);
        Task<List<ToDoListVM>> GetUserTodoAsync(string userId);
        Task<IEnumerable<ToDoItem>> GetFilteredAndSortedToDosAsync(string userId, string searchString, string statusFilter, string sortOrder);
        bool UpdateToDo(ToDoItem toDo);
    }
}
