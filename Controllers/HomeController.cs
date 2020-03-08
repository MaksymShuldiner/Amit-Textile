using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Enums;
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
        public async Task<IActionResult> ShowCategory(string CatId, int page = 1, int EnumParam = 1)
        {
            List<Textile> Textiles = new List<Textile>();
            int count = _context.Categories.Find(CatId).TextilesOfThisCategory.Count();
            int textilesForPage = 9;
            SortingParams param;
            Enum.TryParse(EnumParam.ToString(), out param);
            if (param == 0)
            {
                param = SortingParams.None;
            }
            switch (param)
            {
                case SortingParams.None:
                     Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByAscending:
                    Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory.OrderBy(x => x.Name)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByDescending:
                    Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory.OrderByDescending(x => x.Name)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByAscending:
                    Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory.OrderBy(x => x.Price)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByDescending:
                    Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory.OrderByDescending(x => x.Price)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.RateByAscending:
                    Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory.OrderBy(x => x.Stars)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.ViewsByAscending:
                    Textiles = _context.Categories.Find(CatId).TextilesOfThisCategory.OrderBy(x => x.ViewsCounter)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
            }
            List<ChildCategory> childCategories = _context.Categories.Find(CatId).ChildCategories.ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, textilesForPage);
            CategoriesViewModel model = new CategoriesViewModel()
                {PageViewModel = pageViewModel, childCategories = childCategories, Textiles = Textiles, SortingParams = param};
            return View(model);
        }
    }
}
