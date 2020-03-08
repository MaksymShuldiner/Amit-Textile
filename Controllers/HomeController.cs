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
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ShowCategory(string CatId, int page = 1, int EnumParam = 1)
        {
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            List<Textile> Textiles = new List<Textile>();
            int count = _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
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
                     Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByAscending:
                    Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory.OrderBy(x => x.Name)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByDescending:
                    Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory.OrderByDescending(x => x.Name)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByAscending:
                    Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory.OrderBy(x => x.Price)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByDescending:
                    Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory.OrderByDescending(x => x.Price)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.RateByAscending:
                    Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory.OrderBy(x => x.Stars)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.ViewsByAscending:
                    Textiles = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.TextilesOfThisCategory.OrderBy(x => x.ViewsCounter)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
            }
            List<ChildCategory> childCategories = _context.Categories.FindAsync(Guid.Parse(CatId)).Result.ChildCategories.ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, textilesForPage);
            List<int> pagesCounterList = new List<int>();
            int counterForPrevious = 3;
            for (int i = 1; i <= pageViewModel.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            CategoriesViewModel model = new CategoriesViewModel()
                {PageViewModel = pageViewModel, childCategories = childCategories, Textiles = Textiles, SortingParams = EnumParam, Category = _context.Categories.FindAsync(Guid.Parse(CatId)).Result, PagesCountList = newList.OrderBy(x => x).ToList()};
            return View(model);
        }
    }
}
