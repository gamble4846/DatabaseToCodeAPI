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
using DatabaseToCode.Manager.Interface;
using DatabaseToCode.Manager.Impl;
using DatabaseToCode.Model.QueryController;

namespace DatabaseToCode.API.Controllers
{
    public class QueryController : ControllerBase
    {
        ILog log4Net;
        ValidationResult ValidationResult;
        public IConfiguration Configuration { get; }
        IWebHostEnvironment Env { get; }
        public string ContentRootPath { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
        public CommonFunctions CommonFunctions { get; set; }
        public IQueryManager QueryManager { get; set; }

        public QueryController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, IQueryManager queryManager)
        {
            log4Net = this.Log<QueryController>();
            ValidationResult = new ValidationResult();
            Configuration = configuration;
            Env = env;
            HttpContextAccessor = httpContextAccessor;
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            QueryManager = queryManager;
        }

        [HttpPost]
        [Route("/api/Query/GetModel/")]
        public ActionResult GetQueryModel([FromBody] GetQueryModelParams model)
        {
            try
            {
                return Ok(QueryManager.GetQueryModel(model.Query, model.ClassName, model.Language));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Exception", ex.Message));
            }
        }
    }
}
