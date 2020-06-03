using Buster.Contexts;
using Buster.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Buster.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CategoriesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Category>>> Get()
        {
            return await context.Categories.Include(x => x.Product).ToListAsync();
        }

        [HttpGet("Primero")]
        public async Task<ActionResult<Category>> PrimerRegistro()
        {
            return await context.Categories.Include(x => x.Product).FirstOrDefaultAsync();
        }

        [HttpGet("{Id}",Name ="ConsultById")]
        public async Task<ActionResult<Category>> Get(int Id)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == Id);

            if(category== null) 
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Category category) 
        {
            context.Categories.Add(category);
            context.SaveChangesAsync();

            return new CreatedAtRouteResult("ConsultById", new { Id = category.Id}, category);
        }
        [HttpPut("{id}")]// api/Categories/1
        public ActionResult Put(int Id, [FromBody] Category category) 
        { 
            if(Id != category.Id) 
            {
                return BadRequest();
            }
            context.Entry(category).State = EntityState.Modified;
            context.SaveChangesAsync();
            return Ok();

        }


    }
}
