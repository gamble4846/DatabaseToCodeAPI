using System;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using DatabaseToCode.Utility;

using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using DatabaseToCode.Model;

namespace DatabaseToCode.API.Controllers
{
    public class TokenController : ControllerBase
    {
        ILog log4Net;
        ValidationResult ValidationResult;
        public IConfiguration Configuration { get; }
        IWebHostEnvironment Env { get; }
        public string ContentRootPath { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
        public CommonFunctions CommonFunctions { get; set; }

        public TokenController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env)
        {
            log4Net = this.Log<TokenController>();
            ValidationResult = new ValidationResult();
            Configuration = configuration;
            Env = env;
            HttpContextAccessor = httpContextAccessor;
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
        }

        [HttpPost]
        [Route("/api/CreateToken")]
        public ActionResult CreateToken([FromBody] TokenModel model)
        {
            var jwtSection = Configuration.GetSection("Jwt");
            var Secret = jwtSection.GetValue<String>("Secret");
            var ValidIssuer = jwtSection.GetValue<String>("ValidIssuer");
            var ValidAudience = jwtSection.GetValue<String>("ValidAudience");


            var claims = new[]
            {
                new Claim("TokenData", JsonConvert.SerializeObject( model )),
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(issuer: ValidIssuer, audience: ValidAudience, claims: claims, expires: DateTime.Now.AddYears(1), signingCredentials: signinCredentials);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

            return Ok(new APIResponse(ResponseCode.SUCCESS, "Token Generated", tokenString));
        }

        [Authorize]
        [HttpGet]
        [Route("/api/GetToken")]
        public ActionResult GetToken()
        {
            try
            {
                var tokenData = CommonFunctions.GetTokenData();
                if (tokenData == null)
                {
                    return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Token Not Found", null));
                }
                else
                {
                    return Ok(new APIResponse(ResponseCode.SUCCESS, "Token Recieved", CommonFunctions.GetTokenData()));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, ex.Message, ex));
            }

        }
    }
}
