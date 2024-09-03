using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ToDoListApp1.Models;
using ToDoListApp1.Models.VMs;
using AutoMapper; // AutoMapper namespace ekleyin

namespace ToDoListApp1.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper; // AutoMapper için IMapper alanını ekleyin

        /// <summary>
        /// Constructor ile bağımlılıkları enjekte ediyoruz.
        /// AutoMapper, modeller arasındaki dönüşümleri kolaylaştırmak için kullanılır.
        /// </summary>
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper; // AutoMapper'i başlatın
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Kayıt işlemi. Kullanıcıdan alınan bilgileri User modeline dönüştürür ve kaydeder.
        /// AutoMapper kullanılarak RegisterVM modelinden User modeline otomatik dönüşüm yapılır.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // AutoMapper kullanarak RegisterVM'den User'a dönüşüm yapılıyor
                var user = _mapper.Map<User>(model); // Manuel eşleme yerine AutoMapper kullanılıyor
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Kullanıcı giriş işlemi. Girilen bilgilerle giriş yapılmaya çalışılır.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, isPersistent: model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı.");
            }
            return View(model);
        }

        /// <summary>
        /// Kullanıcı çıkış işlemi. Kullanıcı oturumu sonlandırılır.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
