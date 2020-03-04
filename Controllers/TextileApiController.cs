using System.Linq;
using AmitTextile.Domain.Context;
using Microsoft.AspNetCore.Mvc;

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
        public OkObjectResult GetCategories()
        {
           return new OkObjectResult("s");
        }
        
    }
}