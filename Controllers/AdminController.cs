using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Models;
using AmitTextile.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace AmitTextile.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<User> _userManager;
        private AmitDbContext _context;
        private EmailService _emailService;
        public AdminController(UserManager<User> userManager, AmitDbContext context, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _context = context;
        }

        public async Task<IActionResult> Main()
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateItem(TextileAddModel model)
        {
            Guid Id = new Guid();
            double Discount =
                (Convert.ToDouble(Math.Round(((model.Price - model.Discount) / model.Price), 3)));
            Textile textile = new Textile();
            if (model.ChildCategoryId == "Нет")
            {
                textile = new Textile() { TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId) };
            }
            else
            {
                 textile = new Textile() { TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId), ChildCategoryId = Guid.Parse(model.ChildCategoryId) };
            }
            if (model.MainFile != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(model.MainFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int) model.MainFile.Length);
                }
                Image image = new Image()
                    {Name = model.MainFile.FileName, MainTextileId = Id, ByteImg = imageData, ImageId = Guid.NewGuid()};
                textile.MainImage = image;
                ICollection<Image> Image = new List<Image>();
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        if (model.Files[i] != null)
                        {
                            byte[] imageData1 = null;
                            using (var binaryReader = new BinaryReader(model.Files[i].OpenReadStream()))
                            {
                                imageData1 = binaryReader.ReadBytes((int) model.Files[i].Length);
                            }
                            Image image2 = new Image()
                            {
                                Name = model.Files[i].FileName, TextileId = Id, ByteImg = imageData1,
                                ImageId = Guid.NewGuid()
                            };
                            Image.Add(image2);
                        }
                    }
                    catch
                    {

                    }
                }
                textile.Images = Image;
                ICollection<CharachteristicValues> values = new List<CharachteristicValues>();
                for (int i = 0; i < model.CharacsNames.Length; i++)
                { 
                    values.Add(new CharachteristicValues()
                    {
                        CharachteristicValuesId = Guid.NewGuid(), Value = model.CharacsValues[i],
                        Name = model.CharacsNames[i], TextileId = Id
                    });
                }
                textile.Charachteristics = values;
                await _context.Textiles.AddAsync(textile);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CatAddModel model)
        {
            Guid Id = new Guid();
            Category category = new Category(){CategoryId = Id, Name = model.Name};
            List<ChildCategory> Childs = new List<ChildCategory>();
            foreach (var x in model.ChildCategoryId)
            {
              ChildCategory Category = await _context.ChildCategories.FindAsync(Guid.Parse(x));
              Category.CategoryId = Id;
              Childs.Add(Category);
            }
            category.ChildCategories = Childs;
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }

        [HttpGet("Charachteristics")]
        public async Task<IActionResult> GetCharachteristics()
        {
            return Ok(await _context.Charachteristics.Include(x => x.Values).ToListAsync());
        }

        [HttpGet("Childs")]
        public async Task<IActionResult> GetChilds()
        {
            return Ok(await _context.ChildCategories.Where(x => x.CategoryId == null).ToListAsync());
        }
        [HttpGet("NotFilters")]
        public async Task<IActionResult> GetFilters()
        {
            return Ok(await _context.Charachteristics.Include(x => x.Values).Where(x=>!_context.FilterCharachteristicses.Any(y=>x.CharachteristicId==y.CharachteristicId)).ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> CreateCharachteristic(CharachteristicsAddModel model)
        {
            if (model.Value == null)
            {
                model.Value = new string[] { };
            }

            Guid Id = new Guid();
            ICollection<CharachteristicVariants> variantses = new List<CharachteristicVariants>();
            foreach (var x in model.Value)
            {
                variantses.Add(new CharachteristicVariants(){CharachteristicVariantsId = new Guid(), CharachteristicId = Id, Value = x});
            }
            Charachteristic charact = new Charachteristic() { CharachteristicId = Guid.NewGuid(), Name = model.Name, Values = variantses };
            await _context.Charachteristics.AddAsync(charact);
            await _context.SaveChangesAsync(); 
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> CreateChildCategory(string name)
        {
            Guid Id = new Guid();
            ChildCategory category = new ChildCategory() {ChildCategoryId = Guid.NewGuid(), Name = name };
            await  _context.ChildCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }

        [HttpGet("GetChilds")]
        public async Task<IActionResult> GetChilds(string CatId)
        {
            return Ok(_context.Categories.Include(x=>x.ChildCategories).FirstOrDefaultAsync(x=>x.CategoryId==Guid.Parse(CatId)).Result.ChildCategories);
        }

        [HttpPost]
        public async Task<IActionResult> FilterAdd(string CharactId)
        {
           await _context.FilterCharachteristicses.AddAsync(new FilterCharachteristics()
                {FilterCharachteristicsId = Guid.NewGuid(), CharachteristicId = Guid.Parse(CharactId)});
           await _context.SaveChangesAsync();
           return RedirectToAction("Main", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> SliderAdd(IFormFile[] files)
        {
            List<byte[]> images = new List<byte[]>();
            List<string> names = new List<string>();
            foreach (var x in files)
            {
                byte[] imageData1 = null;
                using (var binaryReader = new BinaryReader(x.OpenReadStream()))
                {
                    imageData1 = binaryReader.ReadBytes((int)x.Length);
                }
                images.Add(imageData1);
                names.Add(x.Name);
            }
            for (int x = 0 ; x<images.Count;x++)
            {
                _context.Images.Add(new Image(){ImageId = Guid.NewGuid(), ByteImg = images[x], SliderId = _context.Sliders.First().SliderId, Name = names[x] });
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Orders(int page = 1)
        {
            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Admin/Orders";
            List<Order> Orders = _context.Orders.Include(x => x.ItemOrders).ThenInclude(x => x.Item).ThenInclude(x=>x.Textile).ThenInclude(x=>x.MainImage).OrderByDescending(x=>x.TimeCreated).Skip((page-1)*5).Take(5).ToList();
            PageViewModel model = new PageViewModel(await _context.Orders.CountAsync(), page,5 );
            List<int> pagesCounterList = new List<int>();
            for (int i = 1; i <= model.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }
            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            return View(new OrdersModel(){Model = model, Orders = Orders, PagesCounterList = newList});
        }

        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems()
        {
            return Ok(await _context.Textiles.ToListAsync());
        }
        [HttpGet("GetCats")]
        public async Task<IActionResult> GetCats()
        {
            return Ok(await _context.Categories.ToListAsync());
        }
        [HttpGet("GetChildss")]
        public async Task<IActionResult> GetChildss()
        {
            return Ok(await _context.ChildCategories.ToListAsync());
        }
        [HttpGet("GetCharacts")]
        public async Task<IActionResult> GetCharacts()
        {
            return Ok(await _context.Charachteristics.ToListAsync());
        }
        [HttpGet("GetFilterss")]
        public async Task<IActionResult> GetFilterss()
        {
            return Ok(await _context.FilterCharachteristicses.Include(x=>x.Charachteristic).ToListAsync());
        }

        [HttpGet("GetSliderImgs")]
        public async Task<IActionResult> GetSliderImgs()
        {
            return Ok(_context.Sliders.Include(x => x.Images).First().Images
                .Select(x => new {StringCode = Convert.ToBase64String(x.ByteImg), Id = x.ImageId}));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTextile(string Id)
        {
            Textile textile = await _context.Textiles.FindAsync(Guid.Parse(Id));
            if (textile != null)
            {
                _context.Textiles.Remove(textile);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteCat(string Id)
        {
            Category cat = await _context.Categories.FindAsync(Guid.Parse(Id));
            if (cat != null)
            {
                _context.Categories.Remove(cat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteChild(string Id)
        {
            ChildCategory cat = await _context.ChildCategories.FindAsync(Guid.Parse(Id));
            if (cat != null)
            {
                _context.ChildCategories.Remove(cat);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteChar(string Id)
        {
            Charachteristic charact = await _context.Charachteristics.FindAsync(Guid.Parse(Id));
            if (charact != null)
            {
                _context.Charachteristics.Remove(charact);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteFilter(string Id)
        {
            FilterCharachteristics filter = await _context.FilterCharachteristicses.FindAsync(Guid.Parse(Id));
            if (filter != null)
            {
                _context.FilterCharachteristicses.Remove(filter);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteSliderImg(string[] Id)
        {
            foreach (var x in Id)
            {
                Image image = await _context.Images.FindAsync(Guid.Parse(x));
                if (image != null)
                {
                    _context.Images.Remove(image);
                    await _context.SaveChangesAsync();
                }
            }
            
            return RedirectToAction("Main", "Admin");
        }
    }
}