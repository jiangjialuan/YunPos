using System;

namespace CySoft.Model.Other
{
    /// <summary>
    /// 引用商品模板
    /// </summary>
    public class CiteGoodsData
    {
        /// <summary>
        /// 引用商品模板
        /// </summary>
        public CiteGoodsData() {}

        /// <summary>
        /// 供应商id
        /// </summary>
        public long? id_gys { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string id_mc { get; set; }

        /// <summary>
        /// 商品名称-组合
        /// </summary>
        public string id_mc_zh { get; set; }

        /// <summary>
        /// 商品sku
        /// </summary>
        public long? id_sku { get; set; }

        /// <summary>
        /// 商品分类
        /// </summary>
        public string id_spfl { get; set; }

        /// <summary>
        /// 用户商品编码
        /// </summary>
        public string bm_Interface { get;set;}
        
        /// <summary>
        /// 启定量
        /// </summary>
        public decimal sl_dh_min { get; set; }

        /// <summary>
        /// 新订货价
        /// </summary>
        public decimal? dj_dh { get; set; }


    }
}

