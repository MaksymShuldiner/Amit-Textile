using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Models;
using AmitTextile.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmitTextile.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<User> _userManager;
        private AmitDbContext _context;
        private EmailService _emailService;
        public AdminController(UserManager<User> userManager, AmitDbContext context, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }

        public async Task<IActionResult> Main()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public async Task<IActionResult> CreateItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(TextileAddModel model)
        {
            return View();
        }
    }
}