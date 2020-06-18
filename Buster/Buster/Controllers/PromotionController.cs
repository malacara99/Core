using Buster.Data;
using Buster.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;

namespace Buster.Controllers
{
    [Route("/api/[Controller]")]
    [ApiController]
    public class PromotionController : ControllerBase
    {
        private readonly PromotionRepository repository;

        public PromotionController(PromotionRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet]
        public async Task<List<PromotionDTO>> Get()
        {
            return await repository.GetAll();
        }

        [HttpGet("{Id}")]
        public async Task<PromotionDTO> Get(int Id)
        {
            var response = await repository.GetById(Id);

            if (response == null)
            {
                NotFound();
            }
            return response;
        }
        [HttpGet("GetById/{Id}")]
        public async Task<PromotionDTO> GetById(int Id)
        {
            var response = await repository.GetById(Id);

            if (response == null)
            {
                NotFound();
            }
            return response;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PromotionDTO promotion)
        {
           await repository.Insert(promotion);
            return NoContent();
        }
    }
}
