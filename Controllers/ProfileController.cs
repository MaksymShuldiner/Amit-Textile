using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using AmitTextile.Models;
using AmitTextile.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmitTextile.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        private EmailService _emailservice;

        public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager,
            AmitDbContext context, EmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailservice = emailService;
        }

        public async Task<IActionResult> ShowFavourite(int page = 1)
        {
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }

            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x =>
            {
                if (x.Textile.IsOnDiscount)
                {
                    sum += (x.Textile.PriceWithDiscount * (decimal)x.ItemsAmount);
                }
                else
                {
                    sum += (x.Textile.Price * (decimal)x.ItemsAmount);
                }

            });
            ViewBag.Sum = sum;
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            ViewBag.BookUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowBook";
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            User user = _context.Users.Include(x => x.UserChosenTextiles).ThenInclude(x => x.Textile)
                .ThenInclude(x => x.ParentCommentReviews).Include(x=>x.UserChosenTextiles).ThenInclude(x=>x.Textile).ThenInclude(x=>x.MainImage)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result;
            List<Textile> Textiles = new List<Textile>();
            user.UserChosenTextiles.ToList().ForEach(x => Textiles.Add(x.Textile));
            PageViewModel Model = new PageViewModel(Textiles.Count, page, 9);
            List<int> pagesCounterList = new List<int>();
            for (int i = 1; i <= Model.TotalPages; i++)
            {
                pagesCounterList.Add(i);
            }

            List<int> newList = pagesCounterList.TakeWhile(x => page - x >= 3 || x - page <= 3).ToList();
            return View(new ShowFavouriteModel()
                {Textiles = Textiles, Model = new PageViewModel(Textiles.Count, page, 9), PagesCounterList = newList});

        }

        [HttpGet]
        public async Task<IActionResult> DeleteFavourite(string TextileId)
        {
            string Url = Request.Headers["Referer"].ToString();
            User user = await _context.Users.Include(x => x.UserChosenTextiles)
                .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
            if (user.UserChosenTextiles.Contains(
                user.UserChosenTextiles.FirstOrDefault(x => x.TextileId == Guid.Parse(TextileId))))
            {
                user.UserChosenTextiles.Remove(
                    user.UserChosenTextiles.FirstOrDefault(x => x.TextileId == Guid.Parse(TextileId)));
                await _userManager.UpdateAsync(user);
                await _context.SaveChangesAsync();
            }
            return Redirect(Url);
        }
        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [Route("passChange")]
        public async Task<IActionResult> ForgotPassword()
        {
            
                if (!User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", "Home");
                }

                string name = User.Identity.Name;
                User user = await _userManager.FindByNameAsync(name);
               
                if ((DateTime.Now - user.LastTimeEmailForPassSent).Hours > 6)
                {
                    user.LastTimeEmailForPassSent = DateTime.Now;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var returningUrl = Url.Action("Index", "Home", new { code = code, name = name },
                                           protocol: HttpContext.Request.Scheme) + "#resetPass";
                await _emailservice.Execute("Password Reset", user.Email, "",
                        $"Для сброса пароля: <a href='{returningUrl}'>link</a>");
                    return Ok();
                }
                else
                {
                    return BadRequest("Отправлять письмо о смене пароля на почту можно лишь раз в 6 часов");
                }
        }
        public async Task<IActionResult> Profile()
        {
            ViewBag.Url = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Home/ShowCategory";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            ViewBag.ProfileUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Profile/Profile";
            List<Item> Items = new List<Item>();
            if (User.Identity.IsAuthenticated)
            {
                Items = _context.Users.Include(x => x.Cart).ThenInclude(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name).Result.Cart.Items.ToList();
            }
            else
            {
                if (Request.Cookies.ContainsKey("Cart"))
                {
                    Items = _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).ThenInclude(x => x.MainImage)
                        .FirstOrDefaultAsync(x => x.NonAuthorizedId == Guid.Parse(Request.Cookies["Cart"])).Result.Items
                        .ToList();
                }
                else { Items = new List<Item>(); }

            }
            ViewBag.Items = Items;
            decimal sum = 0;
            Items.ForEach(x =>
            {
                if (x.Textile.IsOnDiscount)
                {
                    sum += (x.Textile.PriceWithDiscount * (decimal)x.ItemsAmount);
                }
                else
                {
                    sum += (x.Textile.Price * (decimal)x.ItemsAmount);
                }

            });
            ViewBag.Sum = sum;
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                User user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);
                return View(user);

            }

        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword([FromBody]PasswordResetViewModel model)
        {
           
            List<string> errors = new List<string>();
            if (ModelState.IsValid)
            {
                ViewBag.name = model.Name;
                ViewBag.code = model.Code;
                User user = await _userManager.FindByNameAsync(model.Name);
                if (user == null)
                {
                    return BadRequest(new List<string>{ "Неверная ссылка для восстановления пароля или ваша почта не подтверждена" });
                }
                string code = HttpUtility.HtmlDecode(model.Code);
                if (user != null)
                {
                    if (((DateTime.Now) - (_userManager.FindByNameAsync(model.Name).Result.LastTimePassChanged)).Days >
                        1)
                    {
                        var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
                        if (result.Succeeded)
                        {
                            user.LastTimePassChanged = DateTime.Now;
                            _context.Users.Update(user);
                            _context.SaveChangesAsync();
                            await _signInManager.SignOutAsync();
                            return Ok();
                        }
                        else
                        {
                            errors.Add("Неверная ссылка для восстановления пароля");
                        }
                    }
                    else
                    {
                        errors.Add("К сожалению пароль можно восстановить только раз в день ");
                    }
                }
                else
                {
                    errors.Add("Неверная ссылка для восстановления пароля");
                }
            }
            else
            {
                List<string> errorsList2 = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage).ToList();
                foreach (var x in errorsList2)
                {
                    errors.Add(x);
                }
            }
            return BadRequest(errors);
        }
        [HttpPost("mail")]
        public async Task<IActionResult> ChangeEmail([FromBody]EmailViewModel model)
        {
            List<string> errors = new List<string>();
            if (ModelState.IsValid)
            {
                string name = User.Identity.Name;
                User user = await _userManager.FindByNameAsync(name);
            if ((DateTime.Now - user.LastTimeEmailForEmailSent).Hours > 5)
            {
                if (_context.Users.Any(x => x.Email == model.Email))
                {
                    return BadRequest(new List<string>{ "Пользователь с данной почтой уже зарегистрирован" });
                }
                user.LastTimeEmailForEmailSent = DateTime.Now;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);
                var returningUrl = Url.Action("OnChangingEmail", "Profile",
                    new {code = code, email = model.Email, name = name}, protocol: HttpContext.Request.Scheme); 
                await _emailservice.Execute("Email Reset", name, "",
                    $"Для смены почты: <a href='{returningUrl}'>link</a>");
                return Ok();
            }
            else
            {
                errors.Add("Отправлять письмо о смене почты на почту можно лишь раз в 5 часов");
            }
            }
            else
            {
                List<string> errorsList2 = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage).ToList();
                foreach (var x in errorsList2)
                {
                    errors.Add(x);
                }
            }
            return BadRequest(errors);
        }
        public async Task<IActionResult> OnChangingEmail(string code, string email, string name)
        {
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Fio = _userManager.FindByNameAsync(User.Identity.Name).Result.Fio;
            }
            User user = await _userManager.FindByNameAsync(name);
            if (((DateTime.Now) - (user.LastTimeEmailChanged)).Days >
                1)
            {
                user.UserName = email;
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    return RedirectToAction("Index", "Home");
                }
                var result = await _userManager.ChangeEmailAsync(user, email, code);
                if (result.Succeeded)
                {
                    user.LastTimeEmailChanged = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    await _context.SaveChangesAsync();
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> ResetInfo(InfoViewModel model)
        {
            User user = await _userManager.FindByNameAsync(User.Identity.Name);
            user.Address = model.Address;
            user.Fio = model.Fio;
            user.PhoneNumber = model.PhoneNumber;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return Ok(Request.Headers["Referer"].ToString());
        }

        [HttpPost("ResetPassForAnons")]
        public async Task<IActionResult> ResetPassForAnons([FromBody]EmailViewModel model)
        {
            List<string> errors = new List<string>();
            User user = await _userManager.FindByEmailAsync(model.Email);
            if (user==null)
            {
                return BadRequest(new List<string>{ "Пользователь с данной почтой не зарегистрирован" });
            }
            if (ModelState.IsValid)
            {
                if ((DateTime.Now - user.LastTimeEmailForPassSent).Hours > 5)
                {
                    user.LastTimeEmailForPassSent = DateTime.Now;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var returningUrl = Url.Action("Index", "Home", new {code = code, name = model.Email},
                                           protocol: HttpContext.Request.Scheme) + "#resetPass";
                    await _emailservice.Execute("PasswordForAnonConfirmation", model.Email, "",
                        $"Для смены почты: <a href='{returningUrl}'>link</a>");
                    return Ok();
                }
                else
                {
                    return BadRequest(new List<string>{ "Отправлять письмо о смене почты на почту можно лишь раз в 5 часов" });
                }
            }
            else
            {
                List<string> errorsList2 = ModelState.Values
                    .SelectMany(x => x.Errors)
                    .Select(x => x.ErrorMessage).ToList();
                foreach (var x in errorsList2)
                {
                    errors.Add(x);
                }
            }
            return BadRequest(errors);
        }
        



    }
}