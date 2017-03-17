using CySoft.Model.Flags;
using CySoft.Model.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Pay
{
    /// <summary>
    /// 开通支付日志模型
    /// </summary>
    [Serializable]
    [Table("RegisterPayLog", "RegisterPayLog")]
    public class RegisterPayLog
    {
        public RegisterPayLog()
        {
            rq_create = DateTime.Now;
        }
        /// <summary>
        /// 日志流水
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 子账户编号
        /// </summary>
        public string ledgerno { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public long id_user_master { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        public PayFlag type_action { get; set; }
        /// <summary>
        /// 执行结果
        /// </summary>
        public PayFlag flag_action { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string des_action { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public string bm_error { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string msg_error { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime rq_create  { get; set; }
    }
}
