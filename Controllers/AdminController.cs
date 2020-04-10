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
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";

            if (User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
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
                textile = new Textile() { TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId), PriceWithDiscount = model.Discount, CostWithWholeCost = model.CostWithWholeCost};
            }
            else
            {
                 textile = new Textile() { TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId), ChildCategoryId = Guid.Parse(model.ChildCategoryId) ,PriceWithDiscount = model.Discount, CostWithWholeCost = model.CostWithWholeCost };
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
            }

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
                variantses.Add(new CharachteristicVariants(){CharachteristicVariantsId = Guid.NewGuid(), CharachteristicId = Id, Value = x});
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
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
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
            List<Textile> textiles = _context.Textiles.Include(x => x.Images).Include(x => x.MainImage).Include(x=>x.Charachteristics).ToList();
            return Ok(textiles);
        }
        [HttpGet("GetCats")]
        public async Task<IActionResult> GetCats()
        {
            return Ok(await _context.Categories.Include(x=>x.ChildCategories).ToListAsync());
        }
        [HttpGet("GetChildss")]
        public async Task<IActionResult> GetChildss()
        {
            return Ok(await _context.ChildCategories.ToListAsync());
        }
        [HttpGet("GetCharacts")]
        public async Task<IActionResult> GetCharacts()
        {
            return Ok(await _context.Charachteristics.Include(x=>x.Values).ToListAsync());
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
                .Select(x => new ImgModel(){Id=x.ImageId, StringCode = Convert.ToBase64String(x.ByteImg)}).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTextile(string Id)
        {
            Image image = _context.Images.FirstOrDefault(x => x.MainTextileId == Guid.Parse(Id));
            _context.Images.Remove(image);
            foreach (var x in _context.Images.Where(x=>x.TextileId == Guid.Parse(Id)))
            {
                _context.Images.Remove(x);
            }
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
        [HttpPost("DeleteSlider")]
        public async Task<IActionResult> DeleteSliderImg([FromBody]IdModel model)
        {
            Image image = await _context.Images.FindAsync(Guid.Parse(model.Id));
            if (image != null) {
                    _context.Images.Remove(image);
                    await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost("RedactTex")]
        public async Task<IActionResult> TextileEdit(TextileAddModel model)
        {
            Textile toUpdateTextile = await _context.Textiles.Include(x=>x.MainImage).Include(x=>x.Charachteristics).Include(x=>x.Category).Include(x=>x.ChildCategory).Include(x=>x.Images).FirstOrDefaultAsync(x=>x.TextileId== Guid.Parse(model.Id));
            Guid Id = new Guid();
            double Discount =
                (Convert.ToDouble(Math.Round(((model.Price - model.Discount) / model.Price), 3)));
            Textile textile = new Textile();
            if (model.ChildCategoryId == "Нет")
            {
                textile = new Textile() { TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId), PriceWithDiscount = model.Discount, CostWithWholeCost = model.CostWithWholeCost };
            }
            else
            {
                textile = new Textile() { TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId), ChildCategoryId = Guid.Parse(model.ChildCategoryId), PriceWithDiscount = model.Discount, CostWithWholeCost = model.CostWithWholeCost };
            }
            if (model.MainFile != null && toUpdateTextile.MainImage==null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(model.MainFile.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)model.MainFile.Length);
                }

                Image image = new Image()
                { Name = model.MainFile.FileName, MainTextileId = toUpdateTextile.TextileId, ByteImg = imageData, ImageId = Guid.NewGuid() };
                _context.Images.Add(image);

            }
            ICollection<Image> Images = new List<Image>();
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    if (model.Files[i] != null)
                    {
                        byte[] imageData1 = null;
                        using (var binaryReader = new BinaryReader(model.Files[i].OpenReadStream()))
                        {
                            imageData1 = binaryReader.ReadBytes((int)model.Files[i].Length);
                        }
                        Image image2 = new Image()
                        {
                            Name = model.Files[i].FileName,
                            TextileId = toUpdateTextile.TextileId,
                            ByteImg = imageData1,
                            ImageId = Guid.NewGuid()
                        };
                        Images.Add(image2);
                    }
                }
                catch
                {

                }
            }
            for (int i = toUpdateTextile.Images.Count;i<3;i++)
            {
                try
                {
                    _context.Images.Add(((List<Image>) Images)[i - toUpdateTextile.Images.Count]);
                }
                catch
                {

                }
            }
            ICollection<CharachteristicValues> values = new List<CharachteristicValues>();
            toUpdateTextile.Charachteristics = new List<CharachteristicValues>();
            for (int i = 0; i < model.CharacsNames.Length; i++)
            {
                _context.CharachteristicValues.Add(new CharachteristicValues()
                {
                    CharachteristicValuesId = Guid.NewGuid(),
                    Value = model.CharacsValues[i],
                    Name = model.CharacsNames[i],
                    TextileId = toUpdateTextile.TextileId
                });
            }
            toUpdateTextile.Name = textile.Name;
            toUpdateTextile.CategoryId = textile.CategoryId;
            if (textile.ChildCategoryId == Guid.Empty)
            {
                toUpdateTextile.ChildCategoryId = textile.ChildCategoryId;
            }
            toUpdateTextile.Description = textile.Description;
            toUpdateTextile.CostWithWholeCost = textile.CostWithWholeCost;
            toUpdateTextile.PriceWithDiscount = textile.PriceWithDiscount;
            toUpdateTextile.Discount = textile.Discount;
            toUpdateTextile.IsOnDiscount = textile.IsOnDiscount;
            toUpdateTextile.Price = textile.Price;
            toUpdateTextile.WarehouseAmount = textile.WarehouseAmount; 
            _context.Textiles.Update(toUpdateTextile); 
            _context.SaveChanges();
            return RedirectToAction("Main", "Admin");
        }

        [HttpPost("UpdateCat")]
        public async Task<IActionResult> UpdateCat(CatAddModel model)
        {
            Category category = _context.Categories.Include(x=>x.ChildCategories).FirstOrDefault(x=> x.CategoryId == Guid.Parse(model.Id));
            category.Name = model.Name;
            List<ChildCategory> Childs = new List<ChildCategory>();
            foreach (var x in model.ChildCategoryId)
            {
                ChildCategory Category = await _context.ChildCategories.FindAsync(Guid.Parse(x));
                Childs.Add(Category);
            }

            category.ChildCategories = Childs;
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost("UpdateChildCat")]
        public async Task<IActionResult> UpdateChildCat(ChildCategory childCat)
        {
            ChildCategory category = await _context.ChildCategories.FindAsync(Guid.Parse(childCat.Id));
            category.Name = childCat.Name;
            _context.ChildCategories.Update(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost("UpdateCharacteristics")]
        public async Task<IActionResult> UpdateChar(CharachteristicsAddModel charct)
        {
            Charachteristic charachteristic = _context.Charachteristics.Include(x=>x.Values).FirstOrDefault(x=>x.CharachteristicId == Guid.Parse(charct.Id));
            if (charct.Value == null)
            {
                charct.Value = new string[] { };
            }
            ICollection<CharachteristicVariants> variantses = new List<CharachteristicVariants>();
            charachteristic.Values = new List<CharachteristicVariants>();
            charachteristic.Name = charct.Name;
            _context.Charachteristics.Update(charachteristic);
            _context.SaveChangesAsync();
            foreach (var x in charct.Value)
            {
                CharachteristicVariants variant = new CharachteristicVariants() { CharachteristicVariantsId = Guid.NewGuid(), CharachteristicId = Guid.Parse(charct.Id),Value = x };
                variantses.Add(variant);
                _context.CharachteristicVariants.Add(variant);

            }
            _context.SaveChanges();
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteOrder([FromBody]OrderIdModel model)
        {
            Order order = await _context.Orders.Include(x => x.ItemOrders).ThenInclude(x => x.Item).FirstOrDefaultAsync(x => x.OrderId == Guid.Parse(model.OrderId));
            if (order != null)
            {
                List<Item> items = order.ItemOrders.Select(x => x.Item).ToList();
                foreach (var x in items)
                {
                    x.isBought = true;
                    _context.Items.Update(x);
                }
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}