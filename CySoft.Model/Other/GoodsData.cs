using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Tb;

namespace CySoft.Model.Other
{
    [Serializable]
    public class GoodsData : Tb_Sp
    {
        private string _description = String.Empty;
        private long _id_spfl = 0;
        private string _unit = String.Empty;
        //public List<Tb_Sp_Pic> _sp_pic;
        //public List<SkuData> _sku;

        /// <summary>
        /// 简要描述
        /// </summary>
        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _description = value;
                }
                else
                {
                    _description = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品分类
        /// </summary>
        public Nullable<long> id_spfl
        {
            get
            {
                return _id_spfl;
            }
            set
            {
                if (value.HasValue)
                {
                    _id_spfl = value.Value;
                }
                else
                {
                    _id_spfl = 0;
                }
            }
        }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string unit
        {
            get
            {
                return _unit;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    _unit = value;
                }
                else
                {
                    _unit = String.Empty;
                }
            }
        }

        /// <summary>
        /// 商品图片
        /// </summary>
        public string[] sp_pic { get; set; }

        /// <summary>
        /// 商品 sku
        /// </summary>
        public List<SkuData> sku { get; set; }

        /// <summary>
        /// 采购商 信息列表（自定义采购商价格）
        /// </summary>
        public List<Tb_Sp_Cgs> sp_cgs { get; set; }
        /// <summary>
        /// 商品标签
        /// </summary>
        public List<Tb_Gys_Tag> Tb_Gys_Tag { get; set; }
    }
}
