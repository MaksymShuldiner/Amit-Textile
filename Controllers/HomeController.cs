using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AmitTextile.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace AmitTextile.Controllers
{
    public class HomeController : Controller
    {
        private AmitDbContext _context;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public HomeController(AmitDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> Index(string name = null, string code = null)
        {
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            ViewBag.Urling = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/Index";
            ViewBag.CartUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Cart/AddToCart";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x=>x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }

            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x => sum += (x.Textile.Price * (decimal)x.ItemsAmount));
            ViewBag.Sum = sum;
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            ViewBag.name = name;
            ViewBag.code = code;
            return View();
        }
        [HttpGet]   
        public async Task<IActionResult> ShowCategory(string CatId, Dictionary<string,List<string>> Filter, int page = 1, int EnumParam = 1, string CookieValue = "Grid")
        {
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x=>x.Textile).ThenInclude(x => x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }
                
            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x => sum += (x.Textile.Price * (decimal)x.ItemsAmount));
            ViewBag.Sum = sum;
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            if (!HttpContext.Request.Cookies.ContainsKey("Form"))
            {
                HttpContext.Response.Cookies.Append("Form", CookieValue ,new CookieOptions(){Expires = DateTime.Now.Add(TimeSpan.FromDays(15)), IsEssential = true });
            }
            else
            {
                HttpContext.Response.Cookies.Delete("Form");
                HttpContext.Response.Cookies.Append("Form", CookieValue, new CookieOptions() { Expires = DateTime.Now.Add(TimeSpan.FromDays(15)), IsEssential = true});
            }
            string FilterQuery = Request.Query["Filter"];
            ViewBag.UrlChild = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowChildCategory";
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            List<Textile> Textiles = new List<Textile>();
            int count = 0;
            int textilesForPage = 9;
            SortingParams param;    
            Enum.TryParse(EnumParam.ToString(), out param);
            if (param == 0)
            {
                param = SortingParams.None;
            }
            if (Filter==null)
            {
                switch (param)
                {
                    case SortingParams.None:
                        Textiles = _context.Categories.Include(x=>x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x=>x.TextilesOfThisCategory).ThenInclude(x=>x.MainImage)
                            .FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count = _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
                        break;
                    case SortingParams.LettersByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.OrderBy(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count= _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
                        break;
                    case SortingParams.LettersByDescending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.OrderByDescending(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count= _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();;
                        ;
                        break;
                    case SortingParams.PriceByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.OrderBy(x => x.Price)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count = _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
                        ;
                        break;
                    case SortingParams.PriceByDescending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.OrderByDescending(x => x.Price)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count= _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
                        ;
                        break;
                    case SortingParams.RateByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.OrderBy(x => x.Stars)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count= _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
                        ;
                        break;
                    case SortingParams.ViewsByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory.OrderBy(x => x.ViewsCounter)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        count= _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
                        ;
                        break;
                }
            }
            else
            {
                switch (param)
                {
                    case SortingParams.None:
                        List<Textile> Textile1 = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x=> x.CategoryId==Guid.Parse(CatId)).TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys )
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            } ).ToList();
                            count = Textile1.Count();
                            Textiles = Textile1.Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByAscending:
                        List<Textile> Textile2 = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            })
                            .ToList();
                            count = Textile2.Count();
                                Textiles= Textile2.OrderBy(x => x.Name)
                                    .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByDescending:
                        List<Textile> Textile3 = _context.Categories.Include(x => x.TextilesOfThisCategory)
                            .ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId))
                            .TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value ==
                                            Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }

                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }

                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                            count = Textile3.Count();
                                Textiles=Textile3.OrderByDescending(x => x.Name)
                                    .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByAscending:
                        List<Textile> Textile4 = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).OrderBy(x => x.Price).ToList();
                            count = Textile4.Count();
                            Textiles = Textile4.Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByDescending:
                        List<Textile> Textile5 = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                            count = Textile5.Count();
                                Textiles=Textile5
                                    .OrderByDescending(x => x.Price).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.RateByAscending:
                        List<Textile> Textile6 = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                            count = Textile6.Count();
                                Textiles= Textile6
                                    .OrderBy(x => x.Stars).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.ViewsByAscending:
                        List<Textile> Textile7 = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.MainImage).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                            count = Textile7.Count();
                                Textiles=Textile7
                                    .OrderBy(x => x.ViewsCounter).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                }
            }

            List<ChildCategory> childCategories = _context.Categories.Include(x => x.ChildCategories).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.ChildCategories.ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, textilesForPage);
            List<int> pagesCounterList = new List<int>();
            for (int i = 1; i <= pageViewModel.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }
            List<TextileForFavViewModel> textileForFavViewModels = new List<TextileForFavViewModel>();
            if (User.Identity.IsAuthenticated)
            {
                Textiles.ForEach(x => textileForFavViewModels.Add(new TextileForFavViewModel()
                {
                    Textile = x,
                    isFavourite = _context.Users.Include(x=>x.UserChosenTextiles).FirstOrDefaultAsync(z=>z.UserName==User.Identity.Name).Result.UserChosenTextiles.Any(y=>x.TextileId== y.TextileId)
                }));
            }
            else
            {
                Textiles.ForEach(x => textileForFavViewModels.Add(new TextileForFavViewModel()
                {
                    Textile = x,
                    isFavourite = false
                }));
            }
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            List<FilterCharachteristics> charachteristics = _context.FilterCharachteristicses.Include(x => x.Charachteristic).ThenInclude(x => x.Values).OrderBy(x => x.Charachteristic.Name).ToList();
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            List<FilterCharachteristics> filters = await _context.FilterCharachteristicses.Include(x => x.Charachteristic)
                .ThenInclude(x => x.Values).ToListAsync();
            CategoriesViewModel model = new CategoriesViewModel()
                { PageViewModel = pageViewModel, childCategories = childCategories, Textiles = textileForFavViewModels, SortingParams = EnumParam,FilterQuery = FilterQuery, Category = _context.Categories.FindAsync(Guid.Parse(CatId)).Result, PagesCountList = newList.OrderBy(x => x).ToList(), CookieValue = CookieValue, FilterDictionary = Filter, Charachteristic = charachteristics };
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ShowChildCategory(string ChildCatId, Dictionary<string, List<string>> Filter, int page = 1, int EnumParam = 1, string CookieValue = "Grid")
        {
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }

            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x => sum += (x.Textile.Price * (decimal)x.ItemsAmount));
            ViewBag.Sum = sum;
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            string FilterQuery = Request.Query["Filter"];
            if (!HttpContext.Request.Cookies.ContainsKey("Form"))
            {
                HttpContext.Response.Cookies.Append("Form", CookieValue, new CookieOptions() { Expires = DateTime.Now.Add(TimeSpan.FromDays(15)), IsEssential = true });
            }
            else
            {       
                HttpContext.Response.Cookies.Delete("Form");
                HttpContext.Response.Cookies.Append("Form", CookieValue, new CookieOptions() { Expires = DateTime.Now.Add(TimeSpan.FromDays(15)), IsEssential = true });
            }
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            ViewBag.UrlChild = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowChildCategory";
            List<Textile> Textiles = new List<Textile>();
            int count = 0;
            int textilesForPage = 9;
            SortingParams param;
            Enum.TryParse(EnumParam.ToString(), out param);
            if (param == 0)
            {
                param = SortingParams.None;
            }
            if (Filter == null) { 
            switch (param)
            {
                case SortingParams.None:
                    count= _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x=>x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                       .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByAscending:
                    count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                        Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                            .OrderBy(x => x.Name).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByDescending:
                    count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                        Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                            .OrderByDescending(x => x.Name).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByAscending:
                    count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                        Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                            .OrderBy(x => x.Price).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByDescending:
                    count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                        Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                            .OrderByDescending(x => x.Price).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.RateByAscending:
                    count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                        Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                            .OrderBy(x => x.Stars).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.ViewsByAscending:
                    count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
                        Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                            .OrderBy(x => x.ViewsCounter).Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
            }
            }
            else
            {
                switch (param)
                {
                    case SortingParams.None:
                        List<Textile> Texts = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory)
                            .ThenInclude(x => x.Charachteristics).Include(x=>x.TextilesOfThisChildCategory).ThenInclude(x=>x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage)
                            .FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId))
                            .TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                Textile textile = x;
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(y => y.Name == charact).Value ==
                                            Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }

                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }

                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                        count = Texts.Count();
                        Textiles = Texts.Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;


                    case SortingParams.LettersByAscending:
                        List<Textile> Texts1 = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList(); 
                        count = Texts1.Count();
                        Textiles = Texts1.OrderBy(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByDescending:
                        List<Textile> Texts2 = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                        count = Texts2.Count();
                        Textiles = Texts2.OrderByDescending(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByAscending:
                        List<Textile> Texts3 = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                            count=Texts3.Count();
                                Textiles=Texts3.OrderBy(x => x.Price)
                                .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByDescending:
                        List<Textile> Texts4 = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            })
                            .ToList();
                        count = Texts4.Count();
                        Textiles = Texts4.OrderByDescending(x => x.Price)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.RateByAscending:
                        List<Textile> Texts5 = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory)
                            .ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage)
                            .FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId))
                            .TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value ==
                                            Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }

                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }

                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            })
                            .ToList();
                        count = Texts5.Count();
                        Textiles = Texts5.OrderBy(x => x.Stars)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.ViewsByAscending:
                        List<Textile> Texts6 = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.Charachteristics).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.MainImage).Include(x => x.TextilesOfThisChildCategory).ThenInclude(x => x.ParentCommentReviews).FirstOrDefault(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).TextilesOfThisChildCategory
                            .Where(x =>
                            {
                                List<bool> result = new List<bool>();
                                bool flag = false;
                                foreach (var charact in Filter.Keys)
                                {
                                    for (int i = 0; i < Filter[charact].Count; i++)
                                    {
                                        if (x.Charachteristics.FirstOrDefault(x => x.Name == charact).Value == Filter[charact][i])
                                        {
                                            flag = true;
                                            result.Add(true);
                                            break;
                                        }
                                        else
                                        {
                                            flag = false;
                                        }
                                    }
                                    if (!flag)
                                    {
                                        result.Add(false);
                                    }
                                }
                                if (result.Contains(false))
                                {
                                    return false;
                                }
                                else
                                {
                                    return true;
                                }
                            }).ToList();
                            count = Texts6.Count();
                                Textiles = Texts6.OrderBy(x => x.ViewsCounter)
                                    .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                }
            }
            PageViewModel pageViewModel = new PageViewModel(count, page, textilesForPage);
            List<int> pagesCounterList = new List<int>();
            int counterForPrevious = 3;
            for (int i = 1; i <= pageViewModel.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }
            List<TextileForFavViewModel> textileForFavViewModels = new List<TextileForFavViewModel>();
            if (User.Identity.IsAuthenticated)
            {
                Textiles.ForEach(x => textileForFavViewModels.Add(new TextileForFavViewModel()
                {
                    Textile = x,
                    isFavourite = _context.Users.Include(x => x.UserChosenTextiles).FirstOrDefaultAsync(z => z.UserName == User.Identity.Name).Result.UserChosenTextiles.Any(y => x.TextileId == y.TextileId)
                }));
            }
            else
            {
                Textiles.ForEach(x => textileForFavViewModels.Add(new TextileForFavViewModel()
                {
                    Textile = x,
                    isFavourite = false
                }));
            }
            List<FilterCharachteristics> charachteristics = _context.FilterCharachteristicses.Include(x => x.Charachteristic).ThenInclude(x=> x.Values).OrderBy(x => x.Charachteristic.Name).ToList();
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            ChildCategoriesViewModel model = new ChildCategoriesViewModel()
            { PageViewModel = pageViewModel, Textiles = textileForFavViewModels, SortingParams = EnumParam, Category = _context.ChildCategories.Include(x => x.Category).FirstOrDefault(x=> x.ChildCategoryId == Guid.Parse(ChildCatId)), PagesCountList = newList.OrderBy(x => x).ToList(), CookieValue = CookieValue, FilterDictionary = Filter, Charachteristic = charachteristics, FilterQuery = FilterQuery};
            return View(model);
        }
       
        public async Task<IActionResult> ShowTextile(string TextileId, int page = 1, string Section = "AboutItem" )
        {
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            Textile textile1 = await _context.Textiles.FindAsync(Guid.Parse(TextileId));
            textile1.ViewsCounter += 1; 
            _context.Textiles.Update(textile1);
            await _context.SaveChangesAsync();
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            ViewBag.UrlCat = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            string Fio = _context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name)?.Result?.Fio;
            int commentscount = 9;
            bool x = User.Identity.IsAuthenticated;
            TextileForFavViewModel textile = new TextileForFavViewModel();
            List<ParentCommentReview> parentCommentReviews = new List<ParentCommentReview>();
            List<ParentCommentQuestion> parentCommentQuestions = new List<ParentCommentQuestion>();
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x=>x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x=>x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }

            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x => sum += (x.Textile.Price * (decimal)x.ItemsAmount));
            ViewBag.Sum = sum;
            if (Section == "AboutItem")
            {
                if (User.Identity.IsAuthenticated)
                {
                    textile = new TextileForFavViewModel()
                    {
                       Textile = await _context.Textiles.Include(x => x.Charachteristics)
                            .Include(x => x.ParentCommentReviews).Include(x=>x.MainImage).Include(x=>x.Images)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = _context.Users.Include(x => x.UserChosenTextiles)
                            .FirstOrDefaultAsync(z => z.UserName == User.Identity.Name).Result.UserChosenTextiles
                            .Any(y=>Guid.Parse(TextileId) == y.TextileId)
                    };
                }
                else
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = false
                    };
                }

            }
            if (Section == "Charachteristics")
            {
                if (User.Identity.IsAuthenticated)
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = _context.Users.Include(x => x.UserChosenTextiles)
                            .FirstOrDefaultAsync(z => z.UserName == User.Identity.Name).Result.UserChosenTextiles
                            .Any(y => Guid.Parse(TextileId) == y.TextileId)
                    };
                }
                else
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = false
                    };
                }
            }
            else if (Section == "CommentsReviews")
            {
                if (User.Identity.IsAuthenticated)
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = _context.Users.Include(x => x.UserChosenTextiles)
                            .FirstOrDefaultAsync(z => z.UserName == User.Identity.Name).Result.UserChosenTextiles
                            .Any(y => Guid.Parse(TextileId) == y.TextileId)
                    };
                }
                else
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = false
                    };
                }
                parentCommentReviews = await _context.ParentCommentReviews.Include(x => x.Sender)
                    .Where(x => x.TextileId == Guid.Parse(TextileId)).OrderBy(x=> x.DatePosted).ToListAsync();
            }
            else if (Section == "CommentsQuestions")
            {
                if (User.Identity.IsAuthenticated)
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = _context.Users.Include(x => x.UserChosenTextiles)
                            .FirstOrDefaultAsync(z => z.UserName == User.Identity.Name).Result.UserChosenTextiles
                            .Any(y => Guid.Parse(TextileId) == y.TextileId)
                    };
                }
                else
                {
                    textile = new TextileForFavViewModel()
                    {
                        Textile = await _context.Textiles.Include(x => x.Charachteristics).Include(x => x.MainImage).Include(x => x.Images)
                            .Include(x => x.ParentCommentReviews)
                            .FirstOrDefaultAsync(x => x.TextileId == Guid.Parse(TextileId)),
                        isFavourite = false
                    };
                }
                parentCommentQuestions = await _context.ParentCommentQuestions.Include(x=>x.Sender).Include(x => x.ChildComments).ThenInclude(x=>x.Sender)
                    .Where(x => x.TextileId == Guid.Parse(TextileId)).OrderBy(x => x.DatePosted).ToListAsync();
            }
            if (Section=="AboutItem")
            {
                return View(new TextileViewModel(){Textile = textile, Section = "AboutItem" });
            }
            else if (Section=="Charachteristics")
            {
                return View(new TextileViewModel()
                    {Textile = textile, Section = "Charachteristics" , Fio = Fio});
            }
            else if(Section=="CommentsReviews")
            {
                PageViewModel model = new PageViewModel(parentCommentReviews.Count, page, commentscount);
                parentCommentReviews = parentCommentReviews.Skip((page - 1) * commentscount).Take(commentscount).ToList();
                List<int> pagesCounterList = new List<int>();
                for (int i = 1; i <= model.TotalPages; i++)
                {
                    pagesCounterList.Add(i);
                }
                List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
                return View(new TextileViewModel()
                    { parentCommentReviews = parentCommentReviews, Section = "CommentsReviews",Fio = Fio, PageViewModel = model, Textile = textile, PagesCount = newList });
            }
            else
            {
                PageViewModel model = new PageViewModel(parentCommentQuestions.Count, page, commentscount);
                parentCommentQuestions =
                    parentCommentQuestions.Skip((page - 1) * commentscount).Take(commentscount).ToList();
                List<int> pagesCounterList = new List<int>();
                for (int i = 1; i <= model.TotalPages; i++)
                {
                    pagesCounterList.Add(i);
                }
                List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
                return View(new TextileViewModel()
                    { parentCommentQuestions = parentCommentQuestions, Section = "CommentsQuestions",Fio = Fio, PageViewModel  = model , Textile = textile, PagesCount = newList});
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostCommentReview(string Datetime, string Message, string Stars, string TextileId, string Advantages, string Drawbacks, string Fio)
        {
            if (Stars == null)
            {
                Stars = "0.0";
            }
            DateTime datetime = DateTime.Parse(Datetime);
            Guid Id = Guid.Parse(TextileId);
            await _context.ParentCommentReviews.AddAsync(new ParentCommentReview(){DatePosted = datetime, TextileId = Id,Stars = double.Parse(Stars, System.Globalization.CultureInfo.InvariantCulture),ParentCommentReviewId = Guid.NewGuid(), SenderId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id, Text = Message, Advantages = Advantages, DrawBacks = Drawbacks,Fio = Fio});
            await _context.SaveChangesAsync();
            string Url2 = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile?TextileId={Id}&page=1&Section=CommentsReviews";
            return Redirect(Url2);
        }

        [HttpPost]
        public async Task<IActionResult> ReplyToComment(string Datetime, string Message, string TextileId, int page, string Fio, string ParentId)
        {
            DateTime datetime = DateTime.Parse(Datetime);
            Guid Id = Guid.Parse(TextileId);
            await _context.ChildCommentQuestions.AddAsync(new ChildCommentQuestion(){ChildCommentQuestionId = Guid.NewGuid(), TextileId = Guid.Parse(TextileId), DatePosted = datetime, ParentCommentId = Guid.Parse(ParentId), SenderId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id, Text = Message,Fio = Fio});
            await _context.SaveChangesAsync();
            string Url1 = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile?TextileId={Id}&page=1&Section=CommentsQuestions"; ;
            return Redirect(Url1);
        }
        [HttpPost]
        public async Task<IActionResult> PostCommentQuestion(string Datetime, string Message, string TextileId, string Fio)
        {
            DateTime datetime = DateTime.Parse(Datetime);
            Guid Id = Guid.Parse(TextileId);
            await _context.ParentCommentQuestions.AddAsync(new ParentCommentQuestion(){DatePosted = datetime, TextileId = Guid.Parse(TextileId), SenderId = _userManager.FindByNameAsync(User.Identity.Name).Result.Id, Text = Message, ParentCommentQuestionId = Guid.NewGuid(), Fio = Fio});
            await _context.SaveChangesAsync();
            string Url1 = Url.Action("ShowTextile", "Home",
                new {TextileId = TextileId, page = 1, Section = "CommentsReviews"});
            string Url2 = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile?TextileId={Id}&page=1&Section=CommentsQuestions";
            return Redirect(Url2);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string StringQuery, int page = 1)
        {
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x=>x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }

            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x => sum += (x.Textile.Price * (decimal)x.ItemsAmount));
            ViewBag.Sum = sum;
            ViewBag.UrlCat = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            ViewBag.SearchUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/Search";
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowTextile";
            List<Textile> Textiles = _context.Textiles.Include(x => x.Category).Include(x => x.ChildCategory)
                .Include(x => x.ParentCommentReviews).Include(x=>x.MainImage)
                .Where(x => x.Category.Name.Replace(" ", "").Contains(StringQuery.Replace(" ", "")) || x.ChildCategory.Name.Replace(" ", "").Contains(StringQuery.Replace(" ", "")) ||
                            x.Name.Replace(" ", "").Contains(StringQuery.Replace(" ", ""))).ToList();
            List<TextileForFavViewModel> model = new List<TextileForFavViewModel>();
            if (User.Identity.IsAuthenticated)
            {
                Textiles.ForEach(x =>
                {
                    model.Add(new TextileForFavViewModel(){
                        Textile = x,
                        isFavourite = _context.Users.Include(x => x.UserChosenTextiles)
                            .FirstOrDefaultAsync(z => z.UserName == User.Identity.Name).Result.UserChosenTextiles
                            .Any(y => x.TextileId == y.TextileId)
                    });
                });
            }
            else
            {
                Textiles.ForEach(x =>
                {
                    model.Add(new TextileForFavViewModel()
                    {
                        Textile = x,
                        isFavourite = false
                    });
                });
            }
            PageViewModel pageViewModel = new PageViewModel(Textiles.Count, page, 9);
            List<int> pagesCounterList = new List<int>();
            for (int i = 1; i <= pageViewModel.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            return View(new SearchModel(){PagesCounterList = newList, Textiles = model.Skip((page - 1) * 9).Take(9).ToList(), Model = pageViewModel, StringQuery = StringQuery});
        }
        public async Task<IActionResult> MakeOrder(MakeOrderModel model)
        {
            Guid Id = Guid.NewGuid();
            Order order = new Order();
            if (model.OrderType == "toAdress")
            { 
                order = new Order() { OrderId = Id, Address = model.Address, CardNum = model.CardNum, DepartmentNum = model.DepartmentNum,TimeCreated = DateTime.Now,DepartmentName = model.DepartmentName, Email = model.Email, Fio = model.Fio, PhoneNumber = model.PhoneNumber, isDelivery  = true, isToAddress = true, isPaidByCash = model.isPaidByCash, Sum = model.Sum , isPickup = false};
            }
            else if (model.OrderType == "toDepartment")
            {
                order = new Order() { OrderId = Id, Address = model.Address, CardNum = model.CardNum, DepartmentNum = model.DepartmentNum, TimeCreated = DateTime.Now,DepartmentName = model.DepartmentName, Email = model.Email, Fio = model.Fio, PhoneNumber = model.PhoneNumber, isDelivery = true, isToAddress = false, isPaidByCash = model.isPaidByCash, Sum = model.Sum, isPickup = false};
            }
            else if (model.OrderType == "pickup")
            {
                order = new Order()
                {
                    OrderId = Id, Address = model.Address, CardNum = model.CardNum, DepartmentNum = model.DepartmentNum,
                    DepartmentName = model.DepartmentName, Email = model.Email, Fio = model.Fio,
                    PhoneNumber = model.PhoneNumber, isDelivery = false, isToAddress = false,
                    isPaidByCash = model.isPaidByCash, Sum = model.Sum, isPickup = true,
                    TimeCreated = DateTime.Now
                };
            }
            List<ItemOrder> itemorders = new List<ItemOrder>();
            if (User.Identity.IsAuthenticated)
            {
                Cart cart = _context.Carts.Include(x=>x.Items).FirstOrDefault(x=> x.CartId == _userManager.FindByNameAsync(User.Identity.Name).Result.CartId);
                foreach (var x in cart.Items )
                {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
                    itemorders.Add(new ItemOrder(){OrderId = Id, ItemId = x.ItemId});
                }
            }
            else
            {
                Cart cart = _context.Carts.Include(x=>x.Items).FirstOrDefault(x => x.NonAuthorizedId == Guid.Parse(HttpContext.Request.Cookies["Cart"]));
                if (cart == null)
                {
                    return RedirectToAction("Index", "Home");
                }
                foreach (var x in cart.Items)
                {
                    itemorders.Add(new ItemOrder() { OrderId = Id, ItemId = x.ItemId });
                }
            }
            order.ItemOrders = itemorders;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
