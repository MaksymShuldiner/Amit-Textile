using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AmitTextile.Models;
using Microsoft.EntityFrameworkCore;


namespace AmitTextile.Controllers
{
    public class HomeController : Controller
    {
        private AmitDbContext _context;
        public HomeController(AmitDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> Categories = await _context.Categories.ToListAsync();
            return View(Categories);
        }
        [HttpGet]
        public async Task<IActionResult> ShowCategory(string CatId, int page = 1)
        {
            int count = _context.Categories.Find(CatId).TextilesOfThisCategory.Count();
            int textilesForPage = 9;
            List<Textile> Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory
                .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
            List<ChildCategory> childCategories = _context.Categories.Find(CatId).ChildCategories.ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, textilesForPage);
            CategoriesViewModel model = new CategoriesViewModel()
                {PageViewModel = pageViewModel, childCategories = childCategories, Textiles = Textiles};
            return View(model);
        }
    }
}
