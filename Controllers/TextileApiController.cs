using AmitTextile.Domain;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmitTextile.Controllers
{
    [ApiController]
    [Route("api/textile")]
    public class TextileApiController : Controller
    {
        public AmitDbContext _context;
        public TextileApiController(AmitDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("getCategories")]
        public async Task<OkObjectResult> GetCategories()
        {
            List<Category> Categories = await _context.Categories.ToListAsync();
            return Ok(Categories);
        }

    }
}