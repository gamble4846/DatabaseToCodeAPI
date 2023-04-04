using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseToCode.Model.TablesController
{
    public class TableColumn
    {
        public string ColumnName { get; set; }
        public int OrdinalPosition { get; set; }
        public string ColumnDefault { get; set; }
        public string IsNullable { get; set; }
        public string DataType { get; set; }
    }

    public class vTvShows
    {
        public Guid BreadId { get; set; }
        public Guid ShowId { get; set; }
        public String PrimaryName { get; set; }
        public String OtherNames { get; set; }
        public String Description { get; set; }
        public String Type { get; set; }
        public Boolean IsStared { get; set; } = true;
        public String IMDBID { get; set; }
        public String ReleaseYear { get; set; }
        public String Genres { get; set; }
        public String GenreGUIDs { get; set; }
    }
}
