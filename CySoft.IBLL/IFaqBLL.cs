using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IFaqBLL : ITreeBLL
    {
        /// <summary>
        /// 获取常见问题分页信息
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        PageNavigate QueryServicePage(Hashtable param);
        /// <summary>
        /// 供应商回复客户
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        BaseResult ReplyClient(dynamic entity);
    }
}
