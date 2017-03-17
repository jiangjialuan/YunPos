using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Model.Flags;

namespace CySoft.Model.Other
{
    [Serializable]
    public class JqGrid_Body
    {
        public long id_gys { get; set; }
        public long id_cgs { get; set; }
        public long id_sp { get; set; }
        public long id_sku { get; set; }
        public decimal sl { get; set; }
        public string gg { get; set; }
        public string unit { get; set; }
        public decimal dj { get; set; }
        public decimal sl_dh_min { get; set; }
        // 商品市场价
        public decimal dj_base { get; set; }
        // 商品折扣后的单价
        public decimal dj_old { get; set; }
        // 小计 （折扣后单价*数量）
        public decimal xj { get; set; }
        
    }
}
