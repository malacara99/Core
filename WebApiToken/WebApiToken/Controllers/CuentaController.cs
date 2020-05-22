﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApiToken.Models;

namespace WebApiToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentaController: ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;

        public CuentaController(
            UserManager<ApplicationUser>userManager,
            SignInManager<ApplicationUser>signInManager,
            IConfiguration configuration
            )
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("Crear")]

        public async Task<ActionResult<UserToken>> CreateUser([FromBody]UserInfo model) 
        {
            var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return BuildToken(model,  new List<string>());
            }
            else 
            {
                return BadRequest("Username or password invalid");
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo) 
        {
            var result = await signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password,false, false);
            if (result.Succeeded) 
            {
                var usuario = await userManager.FindByEmailAsync(userInfo.Email);
                var roles = await userManager.GetRolesAsync(usuario);

                return BuildToken(userInfo, roles);
            }
            else 
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return BadRequest(ModelState);
            }

        }

        private UserToken BuildToken(UserInfo userInfo, IList<string>roles)
        {
            var claims = new List<Claim>
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.UniqueName,  userInfo.Email),
                new Claim("miValor","lo que yo quiera"),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };


            foreach(var rol in roles) 
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Tiempo de expiración una hora
            var expiration = DateTime.UtcNow.AddHours(1);

            JwtSecurityToken token = new JwtSecurityToken
            (
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiration,
            signingCredentials: creds);

            return new UserToken
            {
                Expiration = expiration,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }
    }
}
