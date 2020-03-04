using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmitTextile.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, AmitDbContext context )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        
        
    }
}