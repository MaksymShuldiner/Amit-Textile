using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Models;
using AmitTextile.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AmitTextile.Controllers
{
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        private EmailService _emailService;
        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, AmitDbContext context, EmailService emailService )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserModel Model)
        {
            List<string> errorsList = new List<string>();
            if (ModelState.IsValid)
            {
                Guid Id = Guid.NewGuid();
                _context.Carts.Add(new Cart { CartId = Id });
                User user = new User() { Fio = Model.Fio, UserName = Model.Email, Email = Model.Email, CartId = Id};
                var result = await _userManager.CreateAsync(user, Model.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    if (code != null)
                    {
                        var callbackUrl = Url.Action("ConfirmEmail", "Auth", new {code = code, userId = user.Id});
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
        public async Task<IActionResult> ConfirmEmail(string code, string userId)
        {
            if (userId == null && code == null)
            {
                return RedirectToAction("Index", "Home");//страничка что кода/айди пользователя неверный
            }
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var result = _userManager.ConfirmEmailAsync(user, code);
            if (result.IsCompletedSuccessfully)
            {
                return RedirectToAction("Index", "Home");//тут должна быть страничка с галочкой типа регистрация успешна
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            

        }
        
    }
}