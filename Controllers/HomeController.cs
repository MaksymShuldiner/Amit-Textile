﻿using System;
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
using Microsoft.AspNetCore.Http;
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
        public async Task<IActionResult> ShowCategory(string CatId, Dictionary<string,List<string>> Filter, int page = 1, int EnumParam = 1, string CookieValue = "Grid")
        {
            if (!HttpContext.Request.Cookies.ContainsKey("Form"))
            {
                HttpContext.Response.Cookies.Append("Form", CookieValue ,new CookieOptions(){Expires = DateTime.Now.Add(TimeSpan.FromDays(15)), IsEssential = true });
            }
            else
            {
                HttpContext.Response.Cookies.Delete("Form");
                HttpContext.Response.Cookies.Append("Form", CookieValue, new CookieOptions() { Expires = DateTime.Now.Add(TimeSpan.FromDays(15)), IsEssential = true});
            }
            ViewBag.UrlChild = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowChildCategory";
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            List<Textile> Textiles = new List<Textile>();
            int count = _context.Categories.Include(x => x.TextilesOfThisCategory).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.TextilesOfThisCategory.Count();
            int textilesForPage = 12;
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
            }
            else
            {
                switch (param)
                {
                    case SortingParams.None:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x=> x.CategoryId==Guid.Parse(CatId)).TextilesOfThisCategory
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
                            } )
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
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
                            }).OrderBy(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByDescending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
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
                            .OrderByDescending(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
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
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByDescending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
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
                            .OrderByDescending(x => x.Price)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.RateByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
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
                            .OrderBy(x => x.Stars)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.ViewsByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(CatId)).TextilesOfThisCategory
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
                            .OrderBy(x => x.ViewsCounter)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                }
            }

            List<ChildCategory> childCategories = _context.Categories.Include(x => x.ChildCategories).FirstOrDefaultAsync(x => x.CategoryId == Guid.Parse(CatId)).Result.ChildCategories.ToList();
            PageViewModel pageViewModel = new PageViewModel(count, page, textilesForPage);
            List<int> pagesCounterList = new List<int>();
            int counterForPrevious = 3;
            for (int i = 1; i <= pageViewModel.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            List<FilterCharachteristics> filters = await _context.FilterCharachteristicses.Include(x => x.Charachteristic)
                .ThenInclude(x => x.Values).ToListAsync();
            CategoriesViewModel model = new CategoriesViewModel()
                { PageViewModel = pageViewModel, childCategories = childCategories, Textiles = Textiles, SortingParams = EnumParam, Category = _context.Categories.FindAsync(Guid.Parse(CatId)).Result, PagesCountList = newList.OrderBy(x => x).ToList(), CookieValue = CookieValue, FilterDictionary = Filter};
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> ShowChildCategory(string ChildCatId, Dictionary<string, List<string>> Filter, int page = 1, int EnumParam = 1, string CookieValue = "Grid")
        {
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
            ViewBag.UrlChild = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowChildCategory";
            List<Textile> Textiles = new List<Textile>();
                int count = _context.ChildCategories.Include(x => x.TextilesOfThisChildCategory).FirstOrDefaultAsync(x => x.ChildCategoryId == Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.Count();
            int textilesForPage = 12;
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
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory
                       .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByAscending:
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.OrderBy(x => x.Name)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.LettersByDescending:
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.OrderByDescending(x => x.Name)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByAscending:
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.OrderBy(x => x.Price)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.PriceByDescending:
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.OrderByDescending(x => x.Price)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.RateByAscending:
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.OrderBy(x => x.Stars)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
                case SortingParams.ViewsByAscending:
                    Textiles = _context.ChildCategories.FindAsync(Guid.Parse(ChildCatId)).Result.TextilesOfThisChildCategory.OrderBy(x => x.ViewsCounter)
                        .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                    break;
            }
            }
            else
            {
                switch (param)
                {
                    case SortingParams.None:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            }).OrderBy(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.LettersByDescending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            .OrderByDescending(x => x.Name)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.PriceByDescending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            .OrderByDescending(x => x.Price)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.RateByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            .OrderBy(x => x.Stars)
                            .Skip(textilesForPage * (page - 1)).Take(textilesForPage).ToList();
                        break;
                    case SortingParams.ViewsByAscending:
                        Textiles = _context.Categories.Include(x => x.TextilesOfThisCategory).ThenInclude(x => x.Charachteristics).FirstOrDefault(x => x.CategoryId == Guid.Parse(ChildCatId)).TextilesOfThisCategory
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
                            .OrderBy(x => x.ViewsCounter)
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
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            ChildCategoriesViewModel model = new ChildCategoriesViewModel()
            { PageViewModel = pageViewModel, Textiles = Textiles, SortingParams = EnumParam, Category = _context.ChildCategories.Include(x => x.Category).FirstOrDefault(x=> x.ChildCategoryId == Guid.Parse(ChildCatId)), PagesCountList = newList.OrderBy(x => x).ToList(), CookieValue = CookieValue};
            return View(model);
        }
    }
}
