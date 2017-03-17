using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IShippingRecordBLL : IBaseBLL
    {
        /// <summary>
        /// 收货确认_越过出库发货
        /// </summary>
        /// <param name="param">发货状态</param>
        /// <returns></returns>
        BaseResult ConfirmSh(Hashtable param);
        /// <summary>
        /// 批量收货确认
        /// </summary>
        /// <param name="param">出库单信息</param>
        /// <returns></returns>
        BaseResult ConfirmBatch(Hashtable param);
    }
}
