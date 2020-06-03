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
            return await context.Products.Include(x=>x.Category).ToListAsync();
        }

        [HttpGet("{Id}", Name ="ObtenerProducto")]
        public async Task<ActionResult<Product>>Get(int id) 
        {
            var product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) 
            {
                return NotFound(); // 404
            }

            return product;
        }

        [HttpPost]
        public  ActionResult Post([FromBody] Product product) 
        {
            context.Products.Add(product);
            context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerProducto", new { id = product.Id }, product);
        }

        [HttpPut]
        public ActionResult Put(int Id, [FromBody]Product product) 
        {
            if(Id != product.Id) 
            {
                return BadRequest(); 
            }

            context.Entry(product).State = EntityState.Modified;
            context.SaveChangesAsync();
            return Ok();

        }

        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(int Id) 
        {
            var product = await context.Products.FirstOrDefaultAsync(x => x.Id == Id);
            if (product == null) 
            {
                return NotFound();
            }
            context.Products.Remove(product);
            context.SaveChanges();
            return product;
        }
    }
}
