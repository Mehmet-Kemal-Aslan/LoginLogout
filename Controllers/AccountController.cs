using Microsoft.AspNetCore.Mvc;
using LoginLogout.Models;


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
            return View();
        }

        public IActionResult SignUp()
        {
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
    }
}
