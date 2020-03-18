using System;
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

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager,
            AmitDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;

        }

        public async Task<IActionResult> ShowFavourite(int page = 1)
        {
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowBook";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            User user = _context.Users.Include(x => x.UserChosenTextiles).ThenInclude(x => x.Textile)
                .ThenInclude(x => x.ParentCommentReviews)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result;
            List<Textile> Textiles = new List<Textile>();
            user.UserChosenTextiles.ToList().ForEach(x => Textiles.Add(x.Textile));
            PageViewModel Model = new PageViewModel(Textiles.Count, page, 9);
            List<int> pagesCounterList = new List<int>();
            for (int i = 1; i <= Model.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }

            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            return View(new ShowFavouriteModel()
                {Textiles = Textiles, Model = new PageViewModel(Textiles.Count, page, 9), PagesCounterList = newList});

        }

        [HttpGet]
        public async Task<IActionResult> DeleteFavourite(string TextileId)
        {
            string Url = Request.Headers["Referer"].ToString();
            User user = await _context.Users.Include(x => x.UserChosenTextiles)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if (user.UserChosenTextiles.Contains(
                user.UserChosenTextiles.FirstOrDefault(x => x.TextileId == Guid.Parse(TextileId))))
            {
                user.UserChosenTextiles.Remove(
                    user.UserChosenTextiles.FirstOrDefault(x => x.TextileId == Guid.Parse(TextileId)));
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
            return Redirect(Url);
        }
    }
}