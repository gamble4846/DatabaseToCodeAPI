using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;
using FastMember;
using System.Collections;
using DatabaseToCode.Manager.Interface;
using DatabaseToCode.Utility;
using DatabaseToCode.Model;
using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.Manager.Impl
{
    public class QueryManager : IQueryManager
    {
        public IConfiguration Configuration { get; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public CommonFunctions CommonFunctions { get; set; }
        MSSqlDatabase MsSqlDatabase { get; set; }
        DatabaseToCode.DataAccess.SQLServer.Interface.IQueryDataAccess SQLQueryDataAccess { get; set; }
        public TokenModel TokenData { get; set; }
        public string ConnectionString { get; set; }
        public string ServerType { get; set; }

        public QueryManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor, env.ContentRootPath);
            TokenData = CommonFunctions.GetTokenData();
            ConnectionString = TokenData.Servers.Find(x => x.IsSelected).ConnectionString;
            ServerType = TokenData.Servers.Find(x => x.IsSelected).DatabaseType;
        }

        
    }
}

