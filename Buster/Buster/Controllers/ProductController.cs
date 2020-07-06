using AutoMapper;
using Buster.Contexts;
using Buster.Entities;
using Buster.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Buster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme, Roles ="admin")]
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
        public async Task<ActionResult<List<ProductDTO>>> Get() 
        {
            var products = await context.Products.Include(x=>x.Category).ToListAsync();
            var productDto = mapper.Map<List<ProductDTO>>(products);

            return productDto;
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
        public async Task<ActionResult> Post([FromBody] ProductCreationDTO productCreation) 
        {
            var product = mapper.Map<Product>(productCreation);
            context.Products.Add(product);
            await context.SaveChangesAsync();
            var productDTO = mapper.Map<ProductDTO>(product);
            return new CreatedAtRouteResult("ObtenerProducto", new { id = product.Id }, productDTO);
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
        [HttpPut("PutDTO")]//Actualización completa, se requiere el usuario envie todos los datos del recurso para  act
        public async Task<ActionResult> PutDTO(int id, [FromBody] ProductCreationDTO productUpdate)
        {
            var product = mapper.Map<Product>(productUpdate);
            product.Id = id;
            context.Entry(product).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return NoContent();

        }


        [HttpDelete]
        public async Task<ActionResult<Product>> Delete(int Id) 
        {
            var productId = await context.Products.Select(x=> x.Id).FirstOrDefaultAsync(x => x == Id);
            if (productId == default(int)) 
            {
                return NotFound();
            }
            context.Products.Remove(new Product {Id = productId });
            await context.SaveChangesAsync();
            return NoContent();
        }


        [HttpPatch("{Id}")]
        public async Task<ActionResult> Patch(int id, JsonPatchDocument<ProductCreationDTO> jsonPatch) 
        {
            var productDb = await context.Products.FirstOrDefaultAsync(x => x.Id == id);
            if (productDb == null) 
            {
                return NotFound();
            }
            var productDTO = mapper.Map<ProductCreationDTO>(productDb);

            jsonPatch.ApplyTo(productDTO, ModelState);

            mapper.Map(productDTO, productDb);
            var isValid = TryValidateModel(productDb);

            if (!isValid) 
            {
                BadRequest(ModelState);
            }

            
            await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
