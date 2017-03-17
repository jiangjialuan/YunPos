using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class DbrkModel:BasePsModel
    {
    }

    public class DbrkQueryModel
    {
        public Td_Ps_Dbrk_1 dbrk1 { get; set; }
        public List<Td_Ps_Dbrk_2_Query> dbrk2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }

}
