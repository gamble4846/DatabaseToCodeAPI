using System.Security.Cryptography;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using DatabaseToCode.DataAccess.SQLServer.Interface;
using DatabaseToCode.Utility;
using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.DataAccess.SQLServer.Impl
{
    public class TablesDataAccess : ITablesDataAccess
    {
        private MSSqlDatabase MSSqlDatabase { get; set; }
        private CommonFunctions CommonFunctions { get; set; }

        public TablesDataAccess(MSSqlDatabase msSqlDatabase, CommonFunctions commonFunctions)
        {
            MSSqlDatabase = msSqlDatabase;
            CommonFunctions = commonFunctions;
        }

        public List<Table> GetTables()
        {
            var ret = new List<Table>();
            var cmd = this.MSSqlDatabase.Connection.CreateCommand() as SqlCommand;
            cmd.CommandText = @"SELECT TABLE_NAME as TableName, TABLE_TYPE as TableType FROM information_schema.tables";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = UtilityCustom.ConvertReaderToObject<Table>(reader);
                    ret.Add(t);
                }
            }
            return ret;
        }

        public List<TableColumn> GetTableColumns(string TableName)
        {
            var ret = new List<TableColumn>();
            var cmd = this.MSSqlDatabase.Connection.CreateCommand() as SqlCommand;
            cmd.CommandText = @"SELECT COLUMN_NAME as ColumnName, ORDINAL_POSITION as OrdinalPosition, COLUMN_DEFAULT as ColumnDefault, IS_NULLABLE as IsNullable, DATA_TYPE as DataType FROM information_schema.columns WHERE table_name = @TableName;";
            cmd.Parameters.AddWithValue("@TableName", TableName);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var t = UtilityCustom.ConvertReaderToObject<TableColumn>(reader);
                    ret.Add(t);
                }
            }
            return ret;
        }
    }
}

