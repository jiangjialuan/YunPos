using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 易宝省市模型
    /// </summary>
    [Serializable]
    [Table("YeePay_City", "YeePay_City")]
    public class YeePay_City
    {
        public int id { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 省编号
        /// </summary>
        public string bm_province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 市编号
        /// </summary>
        public string bm_city { get; set; }
    }
}
