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
    public enum GoodsRecommendFlag
    {
        //待审核
        CheckPending = 0,

        //不通过
        NoPass = 2,

        //通过
        Pass = 1,

        //作废
        invalid=3
    }
}
