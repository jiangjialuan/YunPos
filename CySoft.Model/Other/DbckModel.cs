using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class DbckModel:BasePsModel
    {
    }

    public class DbckQueryModel
    {
        public Td_Ps_Dbck_1 dbck1 { get; set; }
        public List<Td_Ps_Dbck_2_Query> dbck2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }

}
