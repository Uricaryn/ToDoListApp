using Microsoft.AspNetCore.Identity;
using ToDoListApp1.Context;
using ToDoListApp1.Models;
using ToDoListApp1.Models.Enums;
using ToDoListApp1.Models.VMs;
using Microsoft.EntityFrameworkCore;

namespace ToDoListApp1.Services
{
    public class TodoService : ITodoService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public TodoService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public bool AddToDo(ToDoCreateVM todo, string userId)
        {
            var result = new ToDoItem()
            {
                UserId = userId,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.Status,
                ReminderDate = todo.ReminderDate // Hatırlatıcı tarihini ekledik
            };
            _context.ToDoItems.Add(result);
            return _context.SaveChanges() > 0;
        }


        public bool Delete(int id)
        {
            var toDo = _context.ToDoItems.Find(id);
            if (toDo is not null)
            {
                _context.ToDoItems.Remove(toDo);
                return _context.SaveChanges() > 0;
            }
            return false;
        }

        public async Task<bool> DeleteToDoAsync(int id, string userId)
        {
            var toDo = await _context.ToDoItems.FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);
            if (toDo == null)
            {
                return false;
            }

            _context.ToDoItems.Remove(toDo);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<ToDoItem>> GetFilteredAndSortedToDosAsync(string userId, string searchString, string statusFilter, string sortOrder)
        {
            var toDos = await _context.ToDoItems.Where(b => b.UserId == userId).ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                toDos = toDos.Where(b => b.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!String.IsNullOrEmpty(statusFilter) && Enum.TryParse<Status>(statusFilter, out var status))
            {
                toDos = toDos.Where(b => b.IsCompleted == status).ToList();
            }

            toDos = sortOrder switch
            {
                "title_asc" => toDos.OrderBy(b => b.Title).ToList(),
                "title_desc" => toDos.OrderByDescending(b => b.Title).ToList(),
                "date_asc" => toDos.OrderBy(b => b.CreatedDate).ToList(),
                "date_desc" => toDos.OrderByDescending(b => b.CreatedDate).ToList(),
                _ => toDos.ToList(),
            };

            return toDos;
        }

        public async Task<ToDoItem> GetToDoByIdAsync(int id)
        {
            return await _context.ToDoItems
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<List<ToDoItem>> GetToDoByNameAsync(string toDoTitle)
        {
            return await _context.ToDoItems
                .Where(x => x.Title.Contains(toDoTitle))
                .ToListAsync();
        }

        public async Task<List<ToDoItem>> GetToDoByStatus(Status status)
        {
            return await _context.ToDoItems
                .Where(x => x.IsCompleted == status)
                .ToListAsync();
        }

        public async Task<List<ToDoListVM>> GetUserTodoAsync(string userId)
        {
            return await _context.ToDoItems
         .Where(b => b.UserId == userId)
         .Select(x => new ToDoListVM
         {
             Id = x.Id,
             Title = x.Title,
             Status = x.IsCompleted,
             CreatedDate = x.CreatedDate
         })
         .ToListAsync();
        }

        public async Task<bool> TodoExistAsync(int id)
        {
            return await _context.ToDoItems.AnyAsync(e => e.Id == id);
        }

        public bool UpdateToDo(ToDoItem toDo)
        {
            _context.ToDoItems.Update(toDo);
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> UpdateToDoAsync(ToDoEditVM toDoEditVM, string userId)
        {
            var toDo = await _context.ToDoItems.FirstOrDefaultAsync(b => b.Id == toDoEditVM.Id && b.UserId == userId);
            if (toDo == null)
            {
                return false;
            }

            toDo.Title = toDoEditVM.Title;
            toDo.IsCompleted = toDoEditVM.Status;
            toDo.Description = toDoEditVM.Description;
            toDo.ReminderDate=toDoEditVM.ReminderDate;

            _context.ToDoItems.Update(toDo);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
