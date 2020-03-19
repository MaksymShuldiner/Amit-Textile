using System;
using System.Linq;
using System.Threading.Tasks;
using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmitTextile.Controllers
{
    public class CartController : Controller
    {
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private AmitDbContext _context;
        public CartController(UserManager<User> userManager, SignInManager<User> signInManager, AmitDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public async Task<IActionResult> AddToCart(string TextileId, string Api = "none")
        {
            bool flag = false;
            if (User.Identity.IsAuthenticated)
            {
                Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.UserName == User.Identity.Name);
                foreach (var x in cart.Items)
                {
                    if (x.Textile.TextileId == Guid.Parse(TextileId))
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    if (_context.Textiles.Find(Guid.Parse(TextileId)).WarehouseAmount >= 1) {
                        await _context.Items.AddAsync(new Item() {ItemId = Guid.NewGuid(), CartId = cart.CartId, TextileId = Guid.Parse(TextileId), ItemsAmount = 1});
                        await _context.SaveChangesAsync();
                    }

                }
                else
                {
                    if (_context.Textiles.Find(Guid.Parse(TextileId)).WarehouseAmount > cart.Items.FirstOrDefault(x => x.Textile.TextileId == Guid.Parse(TextileId)).ItemsAmount)
                    {
                        cart.Items.FirstOrDefault(x => x.Textile.TextileId == Guid.Parse(TextileId)).ItemsAmount++;
                        _context.Update(cart);
                        await _context.SaveChangesAsync();
                    }
                }

            }
            else
            {

                if (!HttpContext.Request.Cookies.ContainsKey("Cart"))
                {
                    bool flag1;
                    Guid Id = Guid.NewGuid();
                    Guid CartId = Guid.NewGuid();
                    Cart Cart = new Cart() { CartId = CartId, NonAuthorizedId = Id };
                    await _context.Carts.AddAsync(Cart);
                    await _context.Items.AddAsync(new Item()
                    { ItemId = Guid.NewGuid(), CartId = CartId, TextileId = Guid.Parse(TextileId), ItemsAmount = 1 });
                    await _context.SaveChangesAsync();
                    HttpContext.Response.Cookies.Append("Cart", Id.ToString(), new Microsoft.AspNetCore.Http.CookieOptions() { Expires = DateTime.Now.Add(TimeSpan.FromDays(90)), IsEssential = true });
                }
                else
                {
                    bool flag2 = false;
                    Guid Id = Guid.Parse(HttpContext.Request.Cookies["Cart"]);
                    Cart Cart = await _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).FirstOrDefaultAsync(x => x.NonAuthorizedId == Id);
                    foreach (var x in Cart.Items)
                    {
                        if (x.Textile.TextileId == Guid.Parse(TextileId))
                        {
                            flag2 = true;
                            break;
                        }
                    }
                    if (!flag2)
                    {
                        if (_context.Textiles.Find(Guid.Parse(TextileId)).WarehouseAmount >= 1)
                        {
                            _context.Items.Add(new Item(){CartId = Cart.CartId, ItemId = Guid.NewGuid(), ItemsAmount = 1, TextileId = Guid.Parse(TextileId)});
                            _context.Update(Cart);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        if (_context.Textiles.Find(Guid.Parse(TextileId)).WarehouseAmount > Cart.Items
                                .FirstOrDefault(x => x.Textile.TextileId == Guid.Parse(TextileId)).ItemsAmount)
                        {
                            Cart.Items.FirstOrDefault(x => x.Textile.TextileId == Guid.Parse(TextileId)).ItemsAmount++;
                            _context.Update(Cart);
                            await _context.SaveChangesAsync();
                        }
                        
                    }

                }
            }

            if (Api == "none")
            {
                return Redirect(Request.Headers["Referer"].ToString());
            }
            else
            {
                return Ok();
            }
           
        }
        
        public async Task<IActionResult> MinusItemInCart(string ItemId)
        {
            if (User.Identity.IsAuthenticated)
            {
                Cart cart = await _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).Include(x => x.User)
                    .FirstOrDefaultAsync(x => x.User.UserName == User.Identity.Name);
                Item item = cart.Items.FirstOrDefault(x => x.ItemId == Guid.Parse(ItemId));
                if (item.ItemsAmount==1)
                {
                    cart.Items.Remove(item);
                    _context.Carts.Update(cart);
                }
                else
                {
                    item.ItemsAmount--;
                    _context.Items.Update(item);
                }
                await _context.SaveChangesAsync();
                return Ok();
            }
            else
            {
                Guid Id = Guid.Parse(HttpContext.Request.Cookies["Cart"]);
                Cart Cart = await _context.Carts.Include(x => x.Items).ThenInclude(x => x.Textile).FirstOrDefaultAsync(x => x.NonAuthorizedId == Id);
                Item item = Cart.Items.FirstOrDefault(x => x.ItemId == Guid.Parse(ItemId));
                if (item.ItemsAmount == 1)
                {
                    Cart.Items.Remove(item);
                    _context.Carts.Update(Cart);
                }
                else
                {
                    item.ItemsAmount--;
                    _context.Items.Update(item);
                }
                
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}