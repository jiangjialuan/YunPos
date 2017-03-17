using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 支付日志操作类型与执行结果
    /// </summary>
    [Flags]
    public enum PayFlag
    {

        #region 操作类型
        /// <summary>
        /// 注册
        /// </summary>
        register = 0,
        /// <summary>
        /// 实名认证
        /// </summary>
        certified = 1, 
        #endregion

        #region 操作结果
        /// <summary>
        /// 成功
        /// </summary>
        Done = 2,
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 3,
        /// <summary>
        /// 错误
        /// </summary>
        Error = 4,
        #endregion

        #region 交易类型
        /// <summary>
        /// 收款
        /// </summary>
        receivables = 5,
        /// <summary>
        /// 支付
        /// </summary>
        payment = 6,
        /// <summary>
        /// 转账
        /// </summary>
        transfer = 7,
        /// <summary>
        /// 提现
        /// </summary>
        cash = 8,
        /// <summary>
        /// 充值
        /// </summary>
        recharge = 9,
        #endregion
        /// <summary>
        /// 已支付
        /// </summary>
        SUCCESS= 10,
        /// <summary>
        /// 已失败
        /// </summary>
        FAIL= 11,
        /// <summary>
        /// 待支付
        /// </summary>
        INIT= 12

    }
}
