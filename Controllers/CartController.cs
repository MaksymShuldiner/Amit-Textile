using System.Threading.Tasks;
using AmitTextile.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AmitTextile.Controllers
{
    public class CartController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;

        public CartController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(string TextileId)
        {
            if (User.Identity.IsAuthenticated)
            {

            }

            return View();
        }
    }
}