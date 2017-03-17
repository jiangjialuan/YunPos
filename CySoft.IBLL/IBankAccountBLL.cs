using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IBankAccountBLL : IBaseBLL
    {
        /// <summary>
        /// 设置供应商默认付款账号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult SetDefault(Hashtable param);
        /// <summary>
        /// 获取供应商默认付款账号
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult GetDefault(Hashtable param);
    }
}
