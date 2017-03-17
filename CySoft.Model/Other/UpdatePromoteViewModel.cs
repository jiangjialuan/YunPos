using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class UpdatePromoteViewModel
    {
        public Td_Promote_1 Promote1 { get; set; }
        public List<Td_Promote_2_Query> Promote2S { get; set; }

        public List<Td_Promote_Shop> Shops { get; set; }
    }
}
