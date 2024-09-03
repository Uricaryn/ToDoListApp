using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ToDoListApp1.Models;
using ToDoListApp1.Models.Enums;
using ToDoListApp1.Models.VMs;
using ToDoListApp1.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace ToDoListApp1.Controllers
{
    [Authorize]
    public class ToDoController : Controller
    {
        private readonly ITodoService _toDoService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper; // _mapper alanını tanımlayın

        /// <summary>
        /// Dependency injection ile çalışan constructor.
        /// AutoMapper, modeller ile view modeller arasındaki dönüşümleri otomatikleştirir,
        /// kod tekrarını azaltır ve bakımı kolaylaştırır.
        /// </summary>
        public ToDoController(ITodoService toDoService, UserManager<User> userManager, IMapper mapper)
        {
            _toDoService = toDoService;
            _userManager = userManager;
            _mapper = mapper; // _mapper alanını başlatın
        }

        /// <summary>
        /// ToDo öğelerini filtreleme ve sıralama seçenekleri ile birlikte görüntüler.
        /// AutoMapper, ToDo entity'lerini ToDoListVM view modellerine dönüştürür,
        /// bu da kodu basitleştirir ve manuel eşleme hatalarını önler.
        /// </summary>
        public async Task<IActionResult> Index(string searchString, string statusFilter, string sortOrder)
        {
            var userId = _userManager.GetUserId(User);

            var toDos = await _toDoService.GetFilteredAndSortedToDosAsync(userId, searchString, statusFilter, sortOrder);

            // AutoMapper kullanarak ToDoListVM'e dönüştürme işlemi
            var toDoListVM = _mapper.Map<List<ToDoListVM>>(toDos); // Manuel eşleme yerine AutoMapper kullanılıyor

            // Status enum'undan durum listesini oluştur
            var statuses = Enum.GetValues(typeof(Status))
                                .Cast<Status>()
                                .Select(s => new SelectListItem
                                {
                                    Value = s.ToString(),
                                    Text = s.ToString()
                                }).ToList();

            var toDoFilterVM = new TodoFilterVM
            {
                CurrentFilter = searchString,
                StatusFilter = statusFilter,
                CurrentSort = sortOrder,
                ToDos = toDoListVM,
                Statuses = statuses,
                TitleSort = sortOrder == "title_asc" ? "title_desc" : "title_asc",
                DateSortParam = sortOrder == "date_asc" ? "date_desc" : "date_asc"
            };

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentStatus"] = statusFilter;
            ViewData["TitleSortParam"] = toDoFilterVM.TitleSort;
            ViewData["DateSortParam"] = toDoFilterVM.DateSortParam;
            ViewData["Statuses"] = statuses;

            return View(toDoFilterVM.ToDos);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ToDoCreateVM toDo)
        {
            var userId = _userManager.GetUserId(User);
            var success = _toDoService.AddToDo(toDo, userId);
            if (success)
            {
                return RedirectToAction("Index");
            }
            return View(toDo);
        }

        /// <summary>
        /// Belirtilen id'ye sahip ToDo öğesini düzenlemek için formu getirir.
        /// AutoMapper, ToDo entity'sini ToDoEditVM view modeline dönüştürür,
        /// manuel eşleme ihtiyacını ortadan kaldırır.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var toDo = await _toDoService.GetToDoByIdAsync(id);

            if (toDo == null)
            {
                return NotFound();
            }

            // AutoMapper kullanarak ToDoEditVM'e dönüştürme işlemi
            var toDoEditVM = _mapper.Map<ToDoEditVM>(toDo); // Manuel eşleme yerine AutoMapper kullanılıyor
            return View(toDoEditVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ToDoEditVM toDoEditVM)
        {
            if (!ModelState.IsValid)
            {
                return View(toDoEditVM);
            }

            var userId = _userManager.GetUserId(User);
            var success = await _toDoService.UpdateToDoAsync(toDoEditVM, userId);

            if (success)
            {
                return RedirectToAction("Index");
            }

            return View(toDoEditVM);
        }

        /// <summary>
        /// Belirtilen id'ye sahip ToDo öğesini silmek için formu getirir.
        /// AutoMapper, ToDo entity'sini ToDoDeleteVM view modeline dönüştürür,
        /// manuel eşleme ihtiyacını ortadan kaldırır.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var toDo = await _toDoService.GetToDoByIdAsync(id);

            if (toDo == null || toDo.UserId != userId)
            {
                return NotFound();
            }

            // AutoMapper kullanarak ToDoDeleteVM'e dönüştürme işlemi
            var toDoDeleteVM = _mapper.Map<ToDoDeleteVM>(toDo); // Manuel eşleme yerine AutoMapper kullanılıyor
            return View(toDoDeleteVM);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            var success = await _toDoService.DeleteToDoAsync(id, userId);

            if (success)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        /// <summary>
        /// Belirtilen id'ye sahip ToDo öğesinin detaylarını görüntüler.
        /// AutoMapper, ToDo entity'sini ToDoDetailsVM view modeline dönüştürür,
        /// manuel eşleme ihtiyacını ortadan kaldırır.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var toDo = await _toDoService.GetToDoByIdAsync(id);

            if (toDo == null || toDo.UserId != userId)
            {
                return NotFound();
            }

            // AutoMapper kullanarak ToDoDetailsVM'e dönüştürme işlemi
            var toDoDetailsVM = _mapper.Map<ToDoDetailsVM>(toDo); // Manuel eşleme yerine AutoMapper kullanılıyor
            return View(toDoDetailsVM);
        }
    }
}
