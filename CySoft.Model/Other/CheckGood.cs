using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Other
{
    /// <summary>
    /// 检查商品模型
    /// </summary>
    public class CheckGood
    {
        /// <summary>
        /// 供应商id
        /// </summary>
        public int id_gys { get; set; }

        /// <summary>
        /// 商品sku
        /// </summary>
        public string id_sku { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public string id_sp { get; set; }

        /// <summary>
        /// 商品编号与规格
        /// </summary>
        public string gg { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int sl { get; set; }

        /// <summary>
        /// 商品数量
        /// </summary>
        public int sl_dh_min { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string unit { get; set; }

        public string dj_old { get; set; }

        public string dj { get; set; }
    }
}
