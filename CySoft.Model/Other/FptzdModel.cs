using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class FptzdModel : BasePsModel
    {

    }

    public class FptzdQueryModel
    {
        public Td_Ps_Fptzd_1 Fptzd1 { get; set; }
        public List<Td_Ps_Fptzd_2_Query> Fptzd2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }


}
