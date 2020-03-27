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
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                        var callbackUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}{Url.Action("ConfirmEmail", "Auth" , new{code = code, userId = user.Id})}";
                        await _emailService.Execute("for some reason", Model.Email, "Подтверди почту", $"<a href ='{callbackUrl}'>link</a>");
                        return Ok("zdarova");
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
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel Model)
        {
            string Url = Request.Headers["Referer"].ToString();
            List<string> errors = new List<string>();
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByEmailAsync(Model.Email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, Model.Password, Model.Remember, false);
                    if (result.Succeeded)
                    {
                        return Ok(Url);
                    }
                    else
                    {
                        errors.Add("Вы ввели неверный логин и/или пароль");
                       
                    }
                }
                else
                {
                    errors.Add("Вы ввели неверный логин и/или пароль");
                }
                
            }
            else
            {
                List<string> errorsList2 = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage).ToList();
                foreach(var x in errorsList2)
                {
                    errors.Add(x);
                }
            }
            return BadRequest(errors);
        }
        [HttpGet("AuthCheck")]
        public async Task<IActionResult> IsAuthentificated()
        {
            bool isAuthenticatedFlag = User.Identity.IsAuthenticated;
            bool isEmailConfirmed;
            if (!isAuthenticatedFlag)
            {
                isEmailConfirmed = false;
            }
            else
            {
                isEmailConfirmed =
                   await _userManager.IsEmailConfirmedAsync(await _userManager.FindByNameAsync(User.Identity.Name));
            }

            return Ok(new { IsEmailConfirmed = isEmailConfirmed, IsAuthentificated = isAuthenticatedFlag });
        }
        [HttpPost]
        [Route("Yep")]
        public async Task<IActionResult> ToFavourite([FromBody]FavouriteModel model)
        {
            if (model.Value == "1")
            {
                User user = await _context.Users.Include(x => x.UserChosenTextiles).FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                user.UserChosenTextiles.Add(new UserChosenTextile() { TextileId = Guid.Parse(model.TextileId), UserId = user.Id });
                try
                {
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return BadRequest(ex);
                }
                return Ok();
            }
            else
            {
                User user = await _context.Users.Include(x => x.UserChosenTextiles)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                if(user.UserChosenTextiles.Contains(user.UserChosenTextiles.FirstOrDefault(x => x.TextileId == Guid.Parse(model.TextileId)))){
                    user.UserChosenTextiles.Remove(
                        user.UserChosenTextiles.FirstOrDefault(x => x.TextileId == Guid.Parse(model.TextileId)));
                    try
                    {
                        await _userManager.UpdateAsync(user);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex);
                    }

                    return Ok();
                }
                else
                {
                    return BadRequest();
                }

            }
        }
        [HttpGet]
        [Route("Remove")]
        public async Task<IActionResult> RemoveFromCart(string ItemId)
        {
            if (User.Identity.IsAuthenticated)
            {
                Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.UserName == User.Identity.Name);
                Item item = cart.Items.FirstOrDefault(x => x.ItemId == Guid.Parse(ItemId));
                if (cart.Items.Contains(item))
                {
                    cart.Items.Remove(item);
                }
                _context.Carts.Update(cart);
                await _context.SaveChangesAsync();
            }
            else
            {
                Guid Id = Guid.Parse(HttpContext.Request.Cookies["Cart"]);
                Cart Cart = await _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).FirstOrDefaultAsync(x => x.NonAuthorizedId == Id);
                Item item = Cart.Items.FirstOrDefault(x => x.ItemId == Guid.Parse(ItemId));
                if (Cart.Items.Contains(item))
                {
                    Cart.Items.Remove(item);
                }
                _context.Carts.Update(Cart);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet("Popular")]
        public async Task<IActionResult> ShowPopular()
        {
            return Ok(await _context.Textiles.OrderBy(x => x.ViewsCounter).Take(12).ToListAsync());
        }
        [HttpGet("NewComings")]
        public async Task<IActionResult> ShowNewComings()
        {
            return Ok(await _context.Textiles.OrderBy(x => x.DateWhenAdded).Take(12).ToListAsync());
        }
        [HttpGet("SellsLeaders")]
        public async Task<IActionResult> SellsLeaders()
        {
            return Ok( _context.Items.Include(x=>x.Textile).OrderBy(x => _context.Items.Where(y=>y.TextileId == x.TextileId && x.isBought).Count()).Take(12).ToList());
        }
        [HttpGet("Discounts")]
        public async Task<IActionResult> Discounts()
        {
            return Ok(_context.Textiles.Where(x=>x.IsOnDiscount).OrderBy(x =>x.Discount).Take(12).ToList());
        }

    }
}