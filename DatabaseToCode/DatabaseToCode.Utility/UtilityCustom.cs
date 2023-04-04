using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using FastMember;
using Microsoft.AspNetCore.Mvc.Filters;
using DatabaseToCode.Model.TablesController;

namespace DatabaseToCode.Utility
{
    public static class UtilityCustom
    {
        public static T ConvertReaderToObject<T>(this SqlDataReader rd) where T : class, new()
        {
            Type type = typeof(T);
            var accessor = TypeAccessor.Create(type);
            var members = accessor.GetMembers();
            var t = new T();
            try
            {
                for (int i = 0; i < rd.FieldCount; i++)
                {
                    if (!rd.IsDBNull(i))
                    {
                        string fieldName = rd.GetName(i);

                        if (members.Any(m => string.Equals(m.Name, fieldName, StringComparison.OrdinalIgnoreCase)))
                        {
                            accessor[t, fieldName] = rd.GetValue(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                var x = ex;
            }


            return t;
        }

        public static dynamic GetFullQueryRow(SqlDataReader rd)
        {
            dynamic RowData = new System.Dynamic.ExpandoObject();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                var currentValue = rd.GetValue(i);
                var currentColumn = rd.GetName(i);
                var currentType = rd.GetFieldType(i);

                if (String.IsNullOrEmpty(currentValue.ToString()))
                {
                    ((IDictionary<String, Object>)RowData).Add(currentColumn, null);
                }
                else
                {
                    ((IDictionary<String, Object>)RowData).Add(currentColumn, currentValue);
                }
            }

            return RowData;
        }

        public static dynamic GetValueFromAuthContextAndVariableName(AuthorizationFilterContext authorizationFilterContext, string VariableName)
        {
            var parameters = authorizationFilterContext.ActionDescriptor.Parameters.ToList();


            var x = 1;
            return null;
        }

        //var t = UtilityCustom.ConvertReaderToObject<LotserialinventoryModel>(reader);

        public static List<TableColumn> TableColumnsFromReader(SqlDataReader rd)
        {
            List<TableColumn> rt = new List<TableColumn>();

            for (int i = 0; i < rd.FieldCount; i++)
            {
                var col = new TableColumn();
                col.ColumnName = rd.GetName(i);
                col.DataType = rd.GetDataTypeName(i).ToString();
                col.OrdinalPosition = i;
                col.IsNullable = "NO";
                col.ColumnDefault = null;
                rt.Add(col);
            }

            return rt;
        }
    }
}

