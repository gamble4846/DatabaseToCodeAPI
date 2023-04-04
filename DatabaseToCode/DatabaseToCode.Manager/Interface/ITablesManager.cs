using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using DatabaseToCode.Utility;

namespace DatabaseToCode.Manager.Interface
{
    public interface ITablesManager
    {
        APIResponse GetTables();
        APIResponse GetTableColumns(string TableName);
        APIResponse GetTableModel(string TableName, string Language);
    }
}

