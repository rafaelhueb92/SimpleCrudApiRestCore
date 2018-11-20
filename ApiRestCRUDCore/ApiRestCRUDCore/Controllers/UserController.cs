using ApiRestCRUDCore.Data.EF;
using ApiRestCRUDCore.Domain.Contracts;
using ApiRestCRUDCore.Domain.Entities;
using Components.Helpers;
using Components.Sign;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace ApiRestCRUDCore.Controllers
{

    [Route("api/v1/[controller]")]
    public class UserController:Controller
    {

        private ApiRestCRUDCoreDataContext context;

        private IUserRepository UserRepo;

        public UserController(ApiRestCRUDCoreDataContext context, IUserRepository UserRepo)
        {
            this.context = context;
            this.UserRepo = UserRepo;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(
           [FromBody] eUser model,
           [FromServices] SigningConfigurations signingConfigurations,
           [FromServices] TokenConfigurations tokenConfigurations)
        {

            eUser User = UserRepo.Log(model.Email, model.Password.Encrypt());

            if (User != null)
            {
                    ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(User.Id.ToString(), "Identifier"),
                    new[]
                    {
                         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                         new Claim(JwtRegisteredClaimNames.NameId, User.Id.ToString())
                    }
                );

                //Token
                DateTime dataCriacao = DateTime.Now;
                DateTime dataExpiracao = dataCriacao + TimeSpan.FromSeconds(tokenConfigurations.Seconds);

                var handler = new JwtSecurityTokenHandler();
                var securityToken = handler.CreateToken(

                    new SecurityTokenDescriptor
                    {
                        Issuer = tokenConfigurations.Issuer,
                        Audience = tokenConfigurations.Audience,
                        SigningCredentials = signingConfigurations.SigningCredentials,
                        Subject = identity,
                        NotBefore = dataCriacao,
                        Expires = dataExpiracao
                    }

                );

                var token = handler.WriteToken(securityToken);

                return Ok(new
                {
                    token,
                    authenticated = true,
                    created = dataCriacao.ToString("yyyy-MM-dd HH:mm:ss"),
                    expiration = dataExpiracao.ToString("yyyy-MM-dd HH:mm:ss")
                });

            }

            return Unauthorized();

        }

        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {

            var Associacoes = UserRepo.Get();

            if (Associacoes == null)
            {
                return NotFound();
            }
            return Ok(Associacoes);

        }

        [Authorize]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {

            var User = UserRepo.Get(id);

            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);

        }

        [Authorize]
        [HttpPost]
        public IActionResult IncluirUser([FromBody] eUser model)
        {

            // Gravar na base

            UserRepo.Add(model);

            return Ok(model);

        }

        [Authorize]
        [HttpPut]
        public IActionResult AlterarUser([FromBody] eUser model)
        {

            if (model == null)
            {
                return BadRequest();
            }

            var User = UserRepo.Get(model.Id);

            if (User == null) return NotFound();

            UserRepo.Update(model);

            return Ok(model);

        }

        [Authorize]
        [HttpDelete]
        public IActionResult ExcluirUser([FromBody] eUser model)
        {

            var User = UserRepo.Get(model.Id);

            if (User == null)
            {
                return NotFound();
            }

            UserRepo.Delete(model);

            return Ok(model);

        }

    }

}