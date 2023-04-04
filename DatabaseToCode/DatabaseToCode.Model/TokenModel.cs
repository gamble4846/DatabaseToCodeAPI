using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breadcrumb.Model
{
    public class TokenModel
    {
        public List<Server> Servers { get; set; }
    }

    public class Server
    {
        public string DatabaseType { get; set; }
        public string ConnectionString { get; set; }
        public bool IsSelected { get; set; }
    }
}
