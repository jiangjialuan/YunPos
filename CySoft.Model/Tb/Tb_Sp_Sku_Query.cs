using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CySoft.Model.Tb
{
    [Serializable]
    [DebuggerDisplay("id_sp_expand_template = {id_sp_expand_template},val = {val}")]
    public class Tb_Sp_Sku_Spec
    {
        public Tb_Sp_Sku_Spec() 
        {
        }

        public Tb_Sp_Sku_Spec(long id_spec_group, string val)
        {
            this.id_spec_group = id_spec_group;
            this.val = val;
        }

        public long id_spec_group { get; set; }
        public string val { get; set; }
    }

    [Serializable]
    [DebuggerDisplay("id_sku = {id_sku},unit = {unit}, dj = {dj}, dj_base = {dj_base}, specListCount = {specList.Count}")]
    public class Tb_Sp_Sku_Query
    {
        public long id{get;set;}//商品Id
        public long id_sku{get;set;}//商品SKU
        public string unit{get;set;}//单位
        public string barcode { get; set; }//条码
        public string bm_Interface{get;set;}//商品编码
        public string photo_min2 {get;set;}//缩略图
        public string photo_min { get; set; }//预览图
        public string photo{get;set;}//原图
        public decimal dj { get; set; }//单价
        public decimal dj_base { get; set; }//市场价
        public decimal sl_kc { get; set; }//库存
        public decimal sl_dh_min { get; set; }//最小订货数量
        public string description { get; set; }//商品简洁        
        public int flag_stop { get; set; }//停用状态
        public int flag_up { get; set; }//上架状态
        public int sort_id { get; set; }//供应商商品排序
        public string keywords { set; get; }//关键字
        public long id_spfl { set; get; }//商品分类ID
        public string name_spfl { set; get; }//商品分类名称
        public int flag_favorites { set; get; }//收藏状态，1-已收藏，0-未收藏
        public IList<Tb_Sp_Sku_Spec> specList { get; set; }

        public IList<Tb_Gys_Sp_Tag_Query> GoodsSkuTagList { get; set; }
    }
}
