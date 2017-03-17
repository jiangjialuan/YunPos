using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;

namespace CySoft.IBLL
{
    public interface IRecieverAddressBLL : IBaseBLL
    {
        /// <summary>
        /// 采购商_设置默认地址
        /// </summary>
        /// <param name="entity">采购商收货地址</param>
        /// <returns></returns>
        BaseResult SettingDefault(dynamic entity);
    }
}
