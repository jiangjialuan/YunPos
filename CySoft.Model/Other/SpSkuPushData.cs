using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;

namespace CySoft.Model.Other
{
    [Serializable]
    public class SpSkuPushData : Tb_Sp_Sku_Push
    {
        //  tgs.id_sp,tgs.zhl,tgs.sl_kc,tgs.sl_kc_bj,
        //       sku.photo_min,sku.photo_min2,
        //(case isnull(tgs.bm_Interface,'') when '' then sku.bm else tgs.bm_Interface end) as bm,
        //sku.unit,fl.name as name_fl,sp.mc as name_sp,sp.id_gys_create,

        //  tgs.dj_base as dj
        //tu.companyname,

        public long id_sp { get; set; }//商品id
        public decimal zhl { get; set; }//
        public decimal sl_kc { get; set; }//
        public decimal sl_kc_bj { get; set; }//
        public string photo_min { get; set; }//图片小
        public string photo_min2 { get; set; }//图片中
        public string bm { get; set; }//编码
        public string unit { get; set; }//单位
        public string name_fl { get; set; }//分类名称
        public string name_sp { get; set; }//商品名称
        public long id_gys_create { get; set; }//商品创建供应商

        public decimal dj { get; set; }//定价
        public string companyname { get; set; }//公司名称


    }
}