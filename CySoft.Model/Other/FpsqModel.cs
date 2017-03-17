using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{

    public class FpsqModel:BasePsModel
    {
    }

    public class FpsqQueryModel
    {
        public Td_Ps_Fpsq_1 Fpsq1 { get; set; }
        public List<Td_Ps_Fpsq_2_Query> Fpsq2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }
}
