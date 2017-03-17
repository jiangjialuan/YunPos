using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IBuyerBLL : IBaseBLL
    {
        /// <summary>
        /// 获取采购商收货地址_分页_列表
        /// </summary>
        /// <param name="cgsid"></param>
        /// <returns></returns>
        BaseResult RecieverAddress(int cgsid);
    }
}
