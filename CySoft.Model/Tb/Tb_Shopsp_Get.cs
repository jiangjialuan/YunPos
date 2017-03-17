#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Tz;
using CySoft.Model.Td;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    public class Tb_Shopsp_Get
    {
        public Tb_Shopsp ShopSP { set; get; }
        public Tb_Spfl Spfl { set; get; }
        public Tb_Shop Shop { set; get; }
        public Tz_Sp_Kc Kc{ set; get; }
        public Td_Sp_Qc Qc { set; get; }
        public List<Tb_Shopsp> ShopSP_DBZ { set; get; }
        public Tb_Shopsp ShopSPMaster { set; get; }


    }
}
