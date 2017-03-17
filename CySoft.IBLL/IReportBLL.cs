using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IReportBLL : IBaseBLL
    {
        /// <summary>
        /// 获取日期报表数据(图示)_分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        PageNavigate GetDataforImg(Hashtable param);
        /// <summary>
        /// 获取客户订单数
        /// </summary>
        /// <param name="param">采购商资料</param>
        /// <returns></returns>
        BaseResult GetCgsCount(Hashtable param);
        /// <summary>
        /// 获取订单统计数据
        /// </summary>
        /// <param name="param">采购商资料</param>
        /// <returns></returns>
        BaseResult GetOrderStatistics(Hashtable param);
    }
}
