using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 订单状态
    /// </summary>
    [Flags]
    public enum OrderFlag
    {
        /// <summary>
        /// 已作废
        /// </summary>
        Invalided =0,

        /// <summary>
        /// 已提交
        /// </summary>
        Submitted=10,

        /// <summary>
        /// 订单审核
        /// </summary>
        OrderCheck=20,

        /// <summary>
        /// 财务审核
        /// </summary>
        FinanceCheck=30,

        /// <summary>
        /// 待出库审核
        /// </summary>
        WaitOutputCheck = 40,

        /// <summary>
        /// 已出库
        /// </summary>
        Outbounded =50,

        /// <summary>
        /// 待发货
        /// </summary>
        WaitDelivery =60,

        /// <summary>
        /// 已发货
        /// </summary>
        Delivered =70,

        /// <summary>
        /// 已收货
        /// </summary>
        Receipted=80,

        /// <summary>
        /// 已删除
        /// </summary>
        Deleted=100
    }
}
