using Microsoft.AspNetCore.Mvc;
using LoginLogout.Models;

namespace LoginLogout.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private readonly DataContext _context;
        private string code = null;

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
                HttpContext.Session.SetInt32("role", User.Role);
                return Redirect("/Home/Index");
            }
            return Unauthorized(401);
        }

        public IActionResult SignUp()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public async Task<IActionResult> Register(Users User, string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                await _context.AddAsync(User);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return Redirect("/Account/SignUp");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ForgotPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public IActionResult ResetPassword()
        {
            if (HttpContext.Session.GetInt32("id").HasValue)
            {
                return Redirect("/Home/Index");
            }
            return View();
        }

        public IActionResult ResetParola(string code, string NewPassword)
        {
            var PasswordCode = _context.PasswordCode.FirstOrDefault(w => w.Code == code);
            if (PasswordCode != null)
            {
                var user = _context.Users.Find(PasswordCode.UserId);
                user.Password = NewPassword;
                _context.Update(user);
                _context.Remove(PasswordCode);
                _context.SaveChanges();
                return Redirect("Index");
            }
            return Redirect("ResetPassword");
        }

        public async Task<IActionResult> Kaydet(Users Model)
        {
            await _context.AddAsync(Model);
            await _context.SaveChangesAsync();

            return Redirect("Index");
        }

        public string Getcode()
        {
            if(code == null)
            {
                Random rand = new Random();
                code = "";
                for(int i=0; i<6; i++)
                {
                    char tmp = Convert.ToChar(rand.Next(48, 58));
                    code += tmp;
                }
            }
            return code;
        }

        public IActionResult SendCode(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                _context.Add(new PasswordCode { UserId = user.Id, Code = Getcode() });
                _context.SaveChanges();
                string text = "<h1>Sıfırlama kodunuz:<h1>" + Getcode() + "";
                string subject = "Parola sıfırlama";
                System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage("example@gmail.com", email, subject, text);
                msg.IsBodyHtml = true;
                System.Net.Mail.SmtpClient sc = new System.Net.Mail.SmtpClient("smtp.gmail.com",587);
                sc.UseDefaultCredentials = false;
                System.Net.NetworkCredential cre = new System.Net.NetworkCredential("example@gmail.com", "password of example@gmail.com");
                sc.Credentials = cre;
                sc.EnableSsl = true;
                sc.Send(msg);
                return Redirect("ResetPassword");
            }
            return Redirect("Index");
        }
    }
}
