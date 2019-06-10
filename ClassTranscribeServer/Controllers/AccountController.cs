﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using ClassTranscribeServer.Models;

namespace ClassTranscribeServer.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<string> Login(ApplicationUser user)
        {

            var result = await _signInManager.PasswordSignInAsync(user.Email, user.Email, false, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == user.Email);
                return await GenerateJwtToken(user.Email, appUser);
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        public async Task<string> Register(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user, user.Email);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(user.Email, user);
            }

            throw new ApplicationException("UNKNOWN_ERROR");
        }

        [HttpPost]
        public async Task<IActionResult> SignIn([FromBody] LoginDto model)
        {
            string jwtToken;
            try
            {
                ApplicationUser user = Validate(model.b2cToken);
                ApplicationUser applicationUser = await _userManager.FindByEmailAsync(user.Email);
                if (applicationUser == null)
                {
                    jwtToken = await Register(user);
                }
                else
                {
                    jwtToken = await Login(user);
                }

            }
            catch (Exception)
            {
                return Unauthorized();
            }

            return Ok(jwtToken);
        }

        private async Task<string> GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT_KEY"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JWT_EXPIRE_DAYS"]));

            var token = new JwtSecurityToken(
                _configuration["JWT_ISSUER"],
                _configuration["JWT_ISSUER"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ApplicationUser Validate(string token)
        {
            string stsDiscoveryEndpoint = "https://" + _configuration["AZURE_B2C_DOMAIN"] +"/" + _configuration["AZURE_B2C_DIRECTORY"] + "/v2.0/.well-known/openid-configuration?p=" + _configuration["AZURE_B2C_SIGNIN_POLICY"];
            
            ConfigurationManager<OpenIdConnectConfiguration> configManager = new ConfigurationManager<OpenIdConnectConfiguration>(stsDiscoveryEndpoint, new OpenIdConnectConfigurationRetriever());

            OpenIdConnectConfiguration config = configManager.GetConfigurationAsync().Result;
            
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = _configuration["AZURE_B2C_CLIENTID"],
                ValidateIssuer = false,
                ValidIssuer = config.Issuer,
                IssuerSigningKeys = config.SigningKeys,
                ValidateLifetime = false
            };


            JwtSecurityTokenHandler tokendHandler = new JwtSecurityTokenHandler();

            SecurityToken jwt;

            var claims = tokendHandler.ValidateToken(token, validationParameters, out jwt);

            JwtSecurityToken j = jwt as JwtSecurityToken;

            var user = new ApplicationUser
            {
                UserName = claims.FindFirst("emails")?.Value,
                Email = claims.FindFirst("emails")?.Value,
                FirstName = claims.FindFirst("given_name")?.Value,
                LastName = claims.FindFirst("family_name")?.Value
            };

            return user;
        }

        public class LoginDto
        {
            [Required]
            public string b2cToken { get; set; }
        }
    }
}