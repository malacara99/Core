using AutoMapper;
using Buster.Contexts;
using Buster.Entities;
using Buster.Models;
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
        private readonly IMapper mapper;

        public ProductController(ApplicationDbContext context,IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> Get() 
        {
            return await context.Products.Include(x=>x.Category).ToListAsync();
        }

        [HttpGet("{Id}", Name ="ObtenerProducto")]
        public async Task<ActionResult<ProductDTO>>Get(int id) 
        {
            var product = await context.Products.Include(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null) 
            {
                return NotFound(); // 404
            }

            var productDTO = mapper.Map<ProductDTO>(product);
            return productDTO;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Product product) 
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            return new CreatedAtRouteResult("ObtenerProducto", new { id = product.Id }, product);
        }

        [HttpPut]
        public async Task<ActionResult> Put(int Id, [FromBody]Product product) 
        {
            if(Id != product.Id) 
            {
                return BadRequest(); 
            }

            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
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
            await context.SaveChangesAsync();
            return product;
        }
    }
}
