using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using DatabaseToCode.DataAccess.SQLServer.Interface;
using DatabaseToCode.Utility;
using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.DataAccess.SQLServer.Impl
{
    public class QueryDataAccess : IQueryDataAccess
    {
        private MSSqlDatabase MSSqlDatabase { get; set; }
        private CommonFunctions CommonFunctions { get; set; }

        public QueryDataAccess(MSSqlDatabase msSqlDatabase, CommonFunctions commonFunctions)
        {
            MSSqlDatabase = msSqlDatabase;
            CommonFunctions = commonFunctions;
        }

        public List<TableColumn> GetColumnsFromQuery(string Query)
        {
            var ret = new List<TableColumn>();
            var cmd = this.MSSqlDatabase.Connection.CreateCommand() as SqlCommand;
            cmd.CommandText = Query;

            using (var reader = cmd.ExecuteReader())
            {
                ret = UtilityCustom.TableColumnsFromReader(reader);
            }
            return ret;
        }
    }
}

