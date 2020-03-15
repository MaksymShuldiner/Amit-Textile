using System;
using System.Linq;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmitTextile.Controllers
{
    public class CartController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        public CartController(UserManager<User> userManager, SignInManager<User> signInManager, AmitDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AddToCart(string TextileId)
        {
            bool flag = false;
            if (User.Identity.IsAuthenticated)
            {
                Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(x=>x.Textile).Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.UserName == User.Identity.Name);
                foreach (var x in cart.Items)
                {
                    if (x.Textile.TextileId == Guid.Parse(TextileId))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    cart.Items.Add(new Item(){CartId = cart.CartId, ItemId = Guid.NewGuid(), ItemsAmount = 1, TextileId = Guid.Parse(TextileId)});
                    _context.Update(cart);
                }
                else
                {
                    cart.Items.FirstOrDefault(x => x.Textile.TextileId == Guid.Parse(TextileId)).ItemsAmount++;
                    _context.Update(cart);
                }
               
            }
            else
            {

            }

            return View();
        }
    }
}