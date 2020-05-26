using Buster.Contexts;
using Buster.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get() 
        {
            return await context.Products.ToListAsync();
        }

        [HttpGet("{Id}", Name ="ObtenerProducto")]
        public async Task<ActionResult<Product>>Get(int id) 
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) 
            {
                return NotFound(); // 404
            }

            return product;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product) 
        {
            context.Products.Add(product);
            context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerProducto", new { id = product.Id }, product);
        }
    }
}
