using CySoft.Model.Flags;
using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 易宝交易记录表
    /// </summary>
    [Serializable]
    [Table("YeePay_Trade", "YeePay_Trade")]
    public class YeePay_Trade
    {
        /// <summary>
        /// 标识
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 主用户
        /// </summary>
        public long id_user_master { get; set; }
        /// <summary>
        /// 子账户商编
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        public PayFlag flag_trade { get; set; }
        /// <summary>
        /// 交易时间
        /// </summary>
        public DateTime rq_trade { get; set; }
        /// <summary>
        /// 交易流水
        /// </summary>
        public string tradeid { get; set; }
        /// <summary>
        /// 交易金额
        /// </summary>
        public decimal je_trade { get; set; }
        /// <summary>
        /// 交易状态
        /// </summary>
        public PayFlag flag_state { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime rq_create { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public long id_user { get; set; }
    }
}
