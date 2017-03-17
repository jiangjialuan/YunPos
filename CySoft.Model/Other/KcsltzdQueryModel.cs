using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class KcsltzdQueryModel
    {
        public Td_Kc_Sltz_1 model1 { get; set; }

        public List<Td_Kc_Sltz_2_Query> model2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }
}
