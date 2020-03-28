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
            if (model.ChildCategoryId=="None")
            {
                ChildCatId = Guid.Empty;
            }
            else
            {
                ChildCatId = Guid.Parse(model.ChildCategoryId);
            }
            double Discount = Convert.ToDouble(Math.Round((model.Price / model.Discount), 3));  
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
                await _context.Textiles.AddAsync(textile);
                await _context.Images.AddAsync(image);
                for (int i = 0; i < 3; i++)
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
                        await _context.Images.AddAsync(image2);
                    }
                }
                for (int i = 0; i < model.CharacsNames.Length; i++)
                {
                   await _context.CharachteristicValues.AddAsync(new CharachteristicValues()
                    {
                        CharachteristicValuesId = Guid.NewGuid(), Value = model.CharacsValues[i],
                        Name = model.CharacsNames[i], TextileId = Id
                    });
                }
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
            await _context.SaveChangesAsync();
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
        [HttpPost]
        public async Task<IActionResult> CreateCharachteristic(CharachteristicsAddModel model)
        {
            Charachteristic charact = new Charachteristic() { CharachteristicId = Guid.NewGuid(), Name = model.Name, Values = (ICollection<CharachteristicVariants>)model.Value.ToList() };
            await _context.Charachteristics.AddAsync(charact);
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(string name)
        {
            Guid Id = new Guid();
            ChildCategory category = new ChildCategory() {ChildCategoryId = Guid.NewGuid(), Name = name };
            await  _context.ChildCategories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Main", "Admin");
        }
    }
}