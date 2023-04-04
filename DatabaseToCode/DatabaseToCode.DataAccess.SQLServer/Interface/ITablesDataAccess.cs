using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.DataAccess.SQLServer.Interface
{
    public interface ITablesDataAccess
    {
        List<Table> GetTables();
        List<TableColumn> GetTableColumns(string TableName);
    }
}

