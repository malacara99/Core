using Buster.Contexts;
using Buster.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Buster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;

        public UsersController(ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        [HttpPost("AssingUserRole")]
        public async Task<ActionResult> AssignUserRole([FromBody]EditRoleDTO editRoleDTO) 
        {
            var user = await userManager.FindByIdAsync(editRoleDTO.UserID);
            if (user == null) 
            {
                return NotFound();
            }

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            await userManager.AddToRoleAsync(user, editRoleDTO.RoleName);

            return Ok();
        }

        [HttpPost("RemoveUserRole")]
        public async Task<ActionResult> RemoveUserRole([FromBody] EditRoleDTO editRoleDTO)
        {
            var user = await userManager.FindByIdAsync(editRoleDTO.UserID);
            if (user == null)
            {
                return NotFound();
            }

            await userManager.RemoveClaimAsync(user, new Claim(ClaimTypes.Role, editRoleDTO.RoleName));
            await userManager.RemoveFromRoleAsync(user, editRoleDTO.RoleName);

            return Ok();
        }
    }
}
