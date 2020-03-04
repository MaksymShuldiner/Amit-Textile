using System;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmitTextile.Models;
using AmitTextile.Services;
using Microsoft.AspNetCore.Identity;

namespace AmitTextile.Controllers
{
    
    [Route("api/textile")]
    public class TextileApiController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        private EmailService _emailService;
        public TextileApiController(UserManager<User> userManager, SignInManager<User> signInManager, AmitDbContext context, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _context = context;
        }

        [HttpGet]
        [Route("getCategories")]
        public async Task<OkObjectResult> GetCategories()
        {
            List<Category> Categories = await _context.Categories.ToListAsync();
            return Ok(Categories);
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody]UserModel Model)
        {
            
            List<string> errorsList = new List<string>();
            if (ModelState.IsValid)
            {
                Guid Id = Guid.NewGuid();
                _context.Carts.Add(new Cart { CartId = Id });
                User user = new User() { Fio = Model.Fio, UserName = Model.Email, Email = Model.Email, CartId = Id };
                var result = await _userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (code != null)
                    {
                        var callbackUrl = Url.Action("ConfirmEmail", "Auth", new { code = code, userId = user.Id });
                        _emailService.Execute("for some reason", "maksym.shuldiner@nure.ua", "Hello bro");
                        return Ok();
                    }
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        if (error.Description.StartsWith("Passwords must have at least one digit ('0'-'9')"))
                        {
                            errorsList.Add("Ваш пароль должен содержать цифры от 0 до 9");
                        }
                        else if (error.Description.StartsWith("User name"))
                        {
                            errorsList.Add("Аккаунт с данной почтой уже зарегистрирован");
                        };
                    }
                }
            }
            else
            {
                List<string> errorsList2 = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage).ToList();

                foreach (var x in errorsList2)
                {
                    errorsList.Add(x);
                }
            }
            return BadRequest(errorsList);
        }

    }
}