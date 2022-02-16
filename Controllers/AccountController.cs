using Microsoft.AspNetCore.Mvc;
using LoginLogout.Models;
using Microsoft.AspNetCore.Http;
using LoginLogout.Filter;

namespace LoginLogout.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly DataContext _context;

        public AccountController(ILogger<AccountController> logger, DataContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public IActionResult Login(string Email, string Password)
        {
            var User = _context.Users.FirstOrDefault(x => x.Email == Email && x.Password == Password);
            if (User != null)
            {
                HttpContext.Session.SetInt32("id", User.Id);
                HttpContext.Session.SetString("fullname", User.Name + " " + User.Surname);
                return Redirect("/Home/Index");
            }
            return Redirect("Index");
        }

        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public async Task<IActionResult> Register(Users User)
        {
            if (User.Id == 0)
            {
                await _context.AddAsync(User);
            }
            else
            {
                _context.Update(User);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }
    }
}
