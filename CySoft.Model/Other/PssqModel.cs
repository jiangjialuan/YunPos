using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class PssqModel:BasePsModel
    {
         
    }
    [Serializable]
    public class PssqQueryModel
    {
        public Td_Ps_Sq_1 Pssq1 { get; set; }
        public List<Td_Ps_Sq_2_Query> Pssq2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }

}
