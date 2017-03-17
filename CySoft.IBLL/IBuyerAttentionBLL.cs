using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IBuyerAttentionBLL : IBaseBLL
    {
        /// <summary>
        /// 获取供应商关注列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        PageNavigate GetNorevPage(Hashtable param);
    }
}
