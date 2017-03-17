using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CySoft.Model.Flags
{
    /// <summary>
    /// 订单来源
    /// </summary>
    [Flags]
    public enum OrderSourceFlag
    {
        /// <summary>
        /// Pc端 新增
        /// </summary>
        PcNew=0,

        /// <summary>
        /// Pc端 购物车
        /// </summary>
        PcCart=1,

        /// <summary>
        /// Pc端 复制
        /// </summary>
        PcClone=2,

        /// <summary>
        /// App端 购物车
        /// </summary>
        AppCart=3,

        /// <summary>
        /// App端 复制
        /// </summary>
        AppClone
    }
}
