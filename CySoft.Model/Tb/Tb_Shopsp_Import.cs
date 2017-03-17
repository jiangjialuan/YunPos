#region Imports
using System;
using System.Diagnostics;
using CySoft.Model.Mapping;
using System.Collections.Generic;
using CySoft.Model.Other;

#endregion

#region
#endregion

namespace CySoft.Model.Tb
{
    public class Tb_Shopsp_Import
    {
        public string barcode { set; get; }
        public string bm { set; get; }
        public string mc { set; get; }
        public string dw { set; get; }
        public string id_spfl { set; get; }
        public string cd { set; get; }
        public string dj_jh { set; get; }
        public string dj_ls { set; get; }
        public string sl_kc_min { set; get; }
        public string sl_kc_max { set; get; }
        public decimal? je_qc { set; get; }
        public decimal? sl_qc { set; get; }
        public string bz { set; get; }
        public string flag_czfs { set; get; }
        public string flag_state { set; get; }
        public string dj_hy { set; get; }
        public string dj_ps { set; get; }
        public string id_gys { set; get; }
        public string yxq { set; get; }
        public string dj_pf { set; get; }
        public string spfl_mc { set; get; }
        public string gys_mc { set; get; }
       
    }

    public class Tb_JhShopsp_Import
    {
        public string id_shopsp { set; get; }
        public string barcode { set; get; }
        public string bm { set; get; }
        public string mc { set; get; }
        public string id_shop { set; get; }
        public string id_spfl { set; get; }
        public string dw { set; get; }
        public decimal? dj_jh { set; get; }
        public decimal? dj_ls { set; get; }
        public string id_kcsp { set; get; }
        public decimal? zhl { set; get; }
        public decimal? dj_pf { set; get; }
        public decimal? sl { set; get; }
        public decimal? dj { set; get; }
        public string bz { set; get; }
        public string sysbz { set; get; }
        public decimal? sl_qm { set; get; }
        public string id_sp { set; get; }

    }

    public class Tb_Shopsp_Import_All
    {
        public List<Tb_Shopsp_Import> SuccessList { set; get; }
        public List<Tb_Shopsp_Import> FailList { set; get; }

    }

    public class Tb_JhShopsp_Import_All
    {
        public List<Tb_JhShopsp_Import> AllList { set; get; }
        public List<Tb_JhShopsp_Import> SuccessList { set; get; }
        public List<Tb_JhShopsp_Import> FailList { set; get; }

    }

    public class KspdShopsp_Import_All
    {
        public List<KspdShopsp_Import> AllList { set; get; }
        public List<KspdShopsp_Import> SuccessList { set; get; }
        public List<KspdShopsp_Import> FailList { set; get; }
    }

    public class KcsltzShopsp_Import_All
    {
        public List<KcsltzShopsp_Import> AllList { set; get; }
        public List<KcsltzShopsp_Import> SuccessList { set; get; }
        public List<KcsltzShopsp_Import> FailList { set; get; }
    }

    public class PsShopsp_Import_All
    {
        public List<Ps_Shopsp_Import> AllList { set; get; }
        public List<Ps_Shopsp_Import> SuccessList { set; get; }
        public List<Ps_Shopsp_Import> FailList { set; get; }
    }

}
