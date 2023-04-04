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
    public class TablesManager : ITablesManager
    {
        public IConfiguration Configuration { get; }
        public IHttpContextAccessor HttpContextAccessor { get; set; }
        public CommonFunctions CommonFunctions { get; set; }
        MSSqlDatabase MsSqlDatabase { get; set; }
        DatabaseToCode.DataAccess.SQLServer.Interface.ITablesDataAccess SQLTablesDataAccess { get; set; }
        public TokenModel TokenData { get; set; }
        public string ConnectionString { get; set; }
        public string ServerType { get; set; }

        public TablesManager(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IHostingEnvironment env)
        {
            Configuration = configuration;
            HttpContextAccessor = httpContextAccessor;
            CommonFunctions = new CommonFunctions(configuration, httpContextAccessor, env.ContentRootPath);
            TokenData = CommonFunctions.GetTokenData();
            ConnectionString = TokenData.Servers.Find(x => x.IsSelected).ConnectionString;
            ServerType = TokenData.Servers.Find(x => x.IsSelected).DatabaseType;
        }

        public APIResponse GetTables()
        {
            switch (ServerType)
            {
                case "SQLServer":
                    MsSqlDatabase = new MSSqlDatabase(ConnectionString);
                    SQLTablesDataAccess = new DatabaseToCode.DataAccess.SQLServer.Impl.TablesDataAccess(MsSqlDatabase, CommonFunctions);

                    var result = SQLTablesDataAccess.GetTables();
                    if (result != null && result.Count > 0)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Records Found", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }

        public APIResponse GetTableColumns(string TableName)
        {
            switch (ServerType)
            {
                case "SQLServer":
                    MsSqlDatabase = new MSSqlDatabase(ConnectionString);
                    SQLTablesDataAccess = new DatabaseToCode.DataAccess.SQLServer.Impl.TablesDataAccess(MsSqlDatabase, CommonFunctions);

                    var result = SQLTablesDataAccess.GetTableColumns(TableName);
                    if (result != null && result.Count > 0)
                    {
                        return new APIResponse(ResponseCode.SUCCESS, "Records Found", result);
                    }
                    else
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }
        }

        public APIResponse GetTableModel(string TableName, string Language)
        {
            List<TableColumn> ColumnsData;
            switch (ServerType)
            {
                case "SQLServer":
                    MsSqlDatabase = new MSSqlDatabase(ConnectionString);
                    SQLTablesDataAccess = new DatabaseToCode.DataAccess.SQLServer.Impl.TablesDataAccess(MsSqlDatabase, CommonFunctions);

                    ColumnsData = SQLTablesDataAccess.GetTableColumns(TableName);
                    if (ColumnsData == null || ColumnsData.Count < 0)
                    {
                        return new APIResponse(ResponseCode.ERROR, "No Records Found");
                    }
                    break;
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Database Type", ServerType);
            }

            switch (Language)
            {
                case "CSharp":
                    string FinalModel = "public class " + TableName + "</br>{</br>";

                    foreach (var column in ColumnsData)
                    {
                        string currentColumn = "&nbsp&nbsp&nbsp&nbsp&nbsp public " + CommonFunctions.GetDataTypeFor(column, "CSharpDataType") + " " + column.ColumnName + " { get; set; } " + CommonFunctions.GetDefaultDataFor(column, "CSharpDataType") + "</br>";
                        FinalModel += currentColumn;
                    }

                    FinalModel += "}</br>";

                    return new APIResponse(ResponseCode.SUCCESS, "Model Created", FinalModel);
                default:
                    return new APIResponse(ResponseCode.ERROR, "Invalid Language", ServerType);
            }
        }
    }
}

