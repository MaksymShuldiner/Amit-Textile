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
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
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