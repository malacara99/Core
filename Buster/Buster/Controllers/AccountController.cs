
using Buster.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Buster.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController :ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private bool isPersistent;

        public AccountController(UserManager<ApplicationUser> userManager
            ,SignInManager<ApplicationUser>signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login(UserInfo userInfo) 
        {
            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, true, true);
            if (result.Succeeded) 
            {
                var usuario = await userManager.FindByEmailAsync(userInfo.Email);
                var roles = await userManager.GetRolesAsync(usuario);

                return buildToken(userInfo, roles);
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }

        }

        [HttpPost("Crear")]
        public async Task<ActionResult<UserToken>> CreateUser([FromBody]UserInfo userInfo) 
        {
            var user = new ApplicationUser { UserName = userInfo.Email, Email = userInfo.Email };
            var result = await userManager.CreateAsync(user, userInfo.Password);
            if (result.Succeeded)
            {
                return buildToken(userInfo, new List<string>());
            }
            else 
            {
                return BadRequest("Username or password Invalid");
            }


        }

        private UserToken buildToken(UserInfo userInfo, IList<string> roles)
        {
            var claims = new List<Claim>
            {
            new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
            new Claim("valor", "AccesoBuster"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach(var rol in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken
            (
                 issuer: null,
                 audience:null,
                 claims:claims,
                 expires:expiration,
                 signingCredentials:creds
             );

            return new UserToken()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }

}
