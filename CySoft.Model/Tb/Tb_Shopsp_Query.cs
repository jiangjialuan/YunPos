#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using CySoft.Model.Tz;
using System.Collections.Generic;
#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    public class Tb_Shopsp_Query : Tb_Shopsp
    {
        public string spfl_mc { set; get; }
        public string shop_mc { set; get; }
        public decimal sl_qm { set; get; }
        public decimal je_qm { set; get; }
        public decimal dj_cb { set; get; }

       
    }


    public class ShopspList_Query : Tb_Shopsp_Query
    {
        public List<Tb_Shopsp> dw_list { set; get; }
        public decimal sl_qm { set; get; }
        public decimal je_qm { set; get; }
        public decimal dj_cb { set; get; }
        public string source { set; get; }

    }


    public class Tb_Shopsp_Query_For_Ps : Tb_Shopsp_Query
    {
        public List<Tb_Shopsp> dw_list { set; get; }

        public decimal sl_qm { set; get; }
        public decimal je_qm { set; get; }
        public decimal dj_cb { set; get; }
        public string id_shopsp_ck { get; set; }
        public string id_kcsp_ck { get; set; }
    }

}
