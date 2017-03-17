using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CySoft.IBLL.Base;
using CySoft.Frame.Core;
using System.Collections;

namespace CySoft.IBLL
{
    public interface IAccountFunctionBLL : IBaseBLL
    {
        /// <summary>
        /// 校检权限
        /// </summary>
        /// <param name="param">用户身份信息</param>
        /// <returns></returns>
        BaseResult Check(Hashtable param);
        /// <summary>
        /// 获取获取主要模块列表
        /// </summary>
        /// <param name="id_user"></param>
        /// <returns></returns>
        BaseResult GetPurview(string id_user);
        BaseResult GetPurview(string id_user,string noUse);
        BaseResult GetUserMenu(string id_user);
    }
}
