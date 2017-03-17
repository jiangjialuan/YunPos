using System;
using System.Collections;
using CySoft.Frame.Core;
using CySoft.IBLL.Base;
using CySoft.Model.Tb;
using CySoft.Model.Other;

namespace CySoft.IBLL
{
    public interface IRoleSettingBLL : IBaseBLL
    {
        /// <summary>
        /// 修改角色名字
        /// </summary>
        /// <param name="param">角色资料</param>
        /// <returns></returns>
        BaseResult ChangeName(Hashtable param);
        /// <summary>
        /// 获取权限模板_列表
        /// </summary>
        /// <returns>功能模块LIST</returns>
        BaseResult GetAllModule();
        /// <summary>
        /// 采购商 或供应商转平台商
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        BaseResult ChangeToPlatform(UpgradeToPlatform model);

        BaseResult GetAllModuleByPlatformRole(Hashtable param);

        BaseResult GetRoleModel(Hashtable param);

        BaseResult UpdateRoleModel(dynamic entity);

        BaseResult GetRoleFunction(Hashtable param);

        BaseResult UpdateRoleFunction(Hashtable param);
    }
}
