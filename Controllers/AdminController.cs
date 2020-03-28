using System;
using System.IO;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Models;
using AmitTextile.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
            Textile textile = new Textile(){TextileId = Id, WarehouseAmount = model.WarehouseAmount, Name = model.Name, Price = model.Price, Description = model.Description, DateWhenAdded = DateTime.Now, Discount  = model.Discount, IsOnDiscount = model.IsOnDiscount};
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
                for (int i = 0; i < 4; i++)
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
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Main", "Admin");
        }


    }
}