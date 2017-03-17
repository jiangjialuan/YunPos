using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CySoft.Model.Td;

namespace CySoft.Model.Other
{
    public class PsrkModel:BasePsModel
    {

        //public string id { get; set; }
        ///// <summary>
        ///// 单号
        ///// </summary>
        //public string dh { get; set; }
        ///// <summary>
        ///// 出库门店ID
        ///// </summary>
        //public string id_shop { get; set; }
        ///// <summary>
        ///// 经办人ID
        ///// </summary>
        //public string id_jbr { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string remark { get; set; }
        ///// <summary>
        ///// JSON Data
        ///// </summary>
        //public string json_data { get; set; }
        ///// <summary>
        ///// 入库门店
        ///// </summary>
        //public string id_shop_rk { get; set; }

        ///// <summary>
        ///// 出库门店
        ///// </summary>
        //public string id_shop_ck { get; set; }
        ///// <summary>
        ///// 金额明细总额
        ///// </summary>
        //public decimal je_mxtotal { get; set; }
        ///// <summary>
        ///// 主用户ID
        ///// </summary>
        //public string id_masteruser { get; set; }
        ///// <summary>
        ///// 创建用户ID
        ///// </summary>
        //public string id_create { get; set; }
        ///// <summary>
        ///// 编辑用户ID
        ///// </summary>
        //public string id_edit { get; set; }
        ///// <summary>
        ///// 开单日期
        ///// </summary>
        //public DateTime rq { get; set; }

        //public string dh_origin { get; set; }

        //public string bm_djlx_origin { get; set; }

        //public string id_bill_origin { get; set; }
    }

    public class PsrkQueryModel
    {
        public Td_Ps_Rk_1 PsRk1 { get; set; }
        public List<Td_Ps_Rk_2_Query> PsRk2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }

    public class PsfprkModel:BasePsModel
    {

        //public string id { get; set; }
        ///// <summary>
        ///// 单号
        ///// </summary>
        //public string dh { get; set; }
        ///// <summary>
        ///// 出库门店ID
        ///// </summary>
        //public string id_shop { get; set; }
        ///// <summary>
        ///// 经办人ID
        ///// </summary>
        //public string id_jbr { get; set; }
        ///// <summary>
        ///// 备注
        ///// </summary>
        //public string remark { get; set; }
        ///// <summary>
        ///// JSON Data
        ///// </summary>
        //public string json_data { get; set; }
        ///// <summary>
        ///// 入库门店
        ///// </summary>
        //public string id_shop_rk { get; set; }

        ///// <summary>
        ///// 出库门店
        ///// </summary>
        //public string id_shop_ck { get; set; }
        ///// <summary>
        ///// 金额明细总额
        ///// </summary>
        //public decimal je_mxtotal { get; set; }
        ///// <summary>
        ///// 主用户ID
        ///// </summary>
        //public string id_masteruser { get; set; }
        ///// <summary>
        ///// 创建用户ID
        ///// </summary>
        //public string id_create { get; set; }
        ///// <summary>
        ///// 编辑用户ID
        ///// </summary>
        //public string id_edit { get; set; }
        ///// <summary>
        ///// 开单日期
        ///// </summary>
        //public DateTime rq { get; set; }
    }

    public class PsfprkQueryModel
    {
        public Td_Ps_Fprk_1 model1 { get; set; }
        public List<Td_Ps_Fprk_2_Query> model2List { get; set; }

        public string zdr { get; set; }

        public string shr { get; set; }
    }

}
