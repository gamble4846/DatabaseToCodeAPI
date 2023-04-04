using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.DataAccess.SQLServer.Interface
{
    public interface IQueryDataAccess
    {
        List<TableColumn> GetColumnsFromQuery(string Query);
    }
}

