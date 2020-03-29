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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            Guid ChildCatId;
            if (model.ChildCategoryId == "Нет")
            {
                ChildCatId = Guid.Empty;
            }
            else
            {
                ChildCatId = Guid.Parse(model.ChildCategoryId);
            }
            double Discount =
                (Convert.ToDouble(Math.Round(((model.Price - model.Discount) / model.Price), 3)));
            Textile textile = new Textile(){TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description,Discount = Discount, DateWhenAdded = DateTime.Now, IsOnDiscount = model.IsOnDiscount, CategoryId = Guid.Parse(model.CategoryId), ChildCategoryId = ChildCatId};
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
            _context.Categories.Update(category);
           
            foreach (var x in model.ChildCategoryId)
            {
              ChildCategory Category = await _context.ChildCategories.FindAsync(Guid.Parse(x));
              Category.CategoryId = Id;
              _context.ChildCategories.Update(Category);
            }
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
    }
}