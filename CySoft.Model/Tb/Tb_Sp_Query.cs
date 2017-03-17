using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CySoft.Model.Tb
{
    [Serializable]
    [DebuggerDisplay("id = {id}, name = {name}, count_sku = {count_sku}, dj = {dj}")]
    public class Tb_Sp_Query
    {
        private string _name_spec_zh = String.Empty;
        private int _flag_Favorites = 0;
        public long id { get; set; }//商品id
        public long id_sku { get; set; }//商品sku
        public string bm_Interface { get; set; }//商品编码
        public string name { get; set; }//商品名称
        public short count_sku { get; set; }//可选规格数量
        public decimal dj { get; set; }//供应商：市场价，客户：起步价
        public string photo { get; set; }//商品图片
        public string photo_min2 { get; set; }//商品图片

        public string alias_gys {get;set;}//供应商
        public int id_gys { get; set; }//供应商ID
        public decimal sl_dh_min { get; set; }//起订量
        public decimal sl_kc { get; set; }//库存
        public int gwc { get; set; }//商品是否已加入购物车
        public int flag_up { get; set; }//商品上架状态
        public decimal gwcsl { get; set; }//购物车商品数量
        /// <summary>
        /// 是否允许查看商品库存
        /// </summary>
        public int kc_flag { get; set; }
        public long id_user_master_gys { get; set; }
        public int flag_Favorites {
            get { return _flag_Favorites; }
            set { _flag_Favorites = value > 0 ? 1 : 0; }
        }//是否收藏，1-是，0-否
        /// <summary>
        /// 商品 扩展属性(sku 规格值)
        /// </summary>
        public List<Tb_Sp_Expand_Query> sp_expand_query
        {
            get;
            set;
        }
        /// <summary>
        /// 规格组合
        /// </summary>
        public string name_spec_zh
        {
            get
            {
                return _name_spec_zh;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _name_spec_zh = value;
                }
                else
                {
                    _name_spec_zh = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品标签
        /// </summary>
        public IList<Tb_Gys_Sp_Tag_Query> sp_tag_list { get; set; }
    }
}
