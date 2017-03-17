using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 易宝银行模型
    /// </summary>
    [Serializable]
    [Table("YeePay_Banks", "YeePay_Banks")]
    public class YeePay_Banks
    {
        public int id { get; set; }
        /// <summary>
        /// 支行
        /// </summary>
        public string sub_bank { get; set; }
        /// <summary>
        /// 联行号
        /// </summary>
        public string bm_bank { get; set; }
        /// <summary>
        /// 总行
        /// </summary>
        public string parent_bank { get; set; }
        /// <summary>
        /// 省
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 市
        /// </summary>
        public string city { get; set; }
    }
}
