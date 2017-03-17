using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CySoft.Model.Td
{
    public class ProcedureQueryModel
    {
        public string proname { set; get; }
        public string proparam { set; get; }
    }

    public class ProcedureOutQueryModel
    {
        public string proname { set; get; }
        public string str { set; get; }
        public string outstr { set; get; }
    }

    public class ProcedureOutQueryResult
    {
        public IList rList { set; get; }
        public string outstr { set; get; }
    }


}
