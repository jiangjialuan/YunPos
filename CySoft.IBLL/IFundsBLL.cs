using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IFundsBLL : IBaseBLL
	{
        /// <summary>
        /// 确认付款
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult PayConfirm(Hashtable param);
        /// <summary>
        /// 供应商_添加付款记录
        /// </summary>
        /// <param name="entity">收款单信息</param>
        /// <returns></returns>
        BaseResult PayForGys(dynamic entity);
        /// <summary>
        /// 取消收款确认
        /// </summary>
        /// <param name="param">收款单资料</param>
        /// <returns></returns>
        BaseResult CancelRecord(Hashtable param);
        /// <summary>
        /// 删除单个付款记录
        /// </summary>
        /// <param name="param">收款单资料</param>
        /// <returns></returns>
        BaseResult Remove(Hashtable param);
	}
}
