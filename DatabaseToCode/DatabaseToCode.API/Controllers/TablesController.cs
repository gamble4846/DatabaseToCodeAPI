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

namespace DatabaseToCode.API.Controllers
{
    public class TablesController : ControllerBase
    {
        ILog log4Net;
        ValidationResult ValidationResult;
        public IConfiguration Configuration { get; }
        IWebHostEnvironment Env { get; }
        public string ContentRootPath { get; set; }
        IHttpContextAccessor HttpContextAccessor { get; set; }
        public CommonFunctions CommonFunctions { get; set; }
        public ITablesManager TablesManager { get; set; }

        public TablesController(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment env, ITablesManager tablesManager)
        {
            log4Net = this.Log<TablesController>();
            ValidationResult = new ValidationResult();
            Configuration = configuration;
            Env = env;
            HttpContextAccessor = httpContextAccessor;
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor);
            TablesManager = tablesManager;
        }

        [HttpGet]
        [Route("/api/Tables/")]
        public ActionResult GetTables()
        {
            try
            {
                return Ok(TablesManager.GetTables());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Exception", ex.Message));
            }
        }

        [HttpGet]
        [Route("/api/Tables/GetColumns/{TableName}")]
        public ActionResult GetTableColumns(string TableName)
        {
            try
            {
                return Ok(TablesManager.GetTableColumns(TableName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Exception", ex.Message));
            }
        }

        [HttpGet]
        [Route("/api/Tables/GetModel/")]
        public ActionResult GetTableModel(string TableName, string Language)
        {
            try
            {
                return Ok(TablesManager.GetTableModel(TableName, Language));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new APIResponse(ResponseCode.ERROR, "Exception", ex.Message));
            }
        }
    }
}
