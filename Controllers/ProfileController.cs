using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmitTextile.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager, AmitDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }
        public async Task<IActionResult> ShowFavourite(int page=1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            User user = _context.Users.Include(x => x.UserChosenTextiles)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result;
            List<Textile> Textiles = new List<Textile>();
            user.UserChosenTextiles.ToList().ForEach(x=>Textiles.Add(x.Textile));
            return View(new ShowFavouriteModel()
                {Textiles = Textiles, Model = new PageViewModel(Textiles.Count, page, 9)});

        }
    }
}