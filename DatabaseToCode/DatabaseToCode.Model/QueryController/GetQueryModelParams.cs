using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseToCode.Model.QueryController
{
    public class GetQueryModelParams
    {
        public string Query { get; set; }
        public string ClassName { get; set; }
        public string Language { get; set; }
    }
}
